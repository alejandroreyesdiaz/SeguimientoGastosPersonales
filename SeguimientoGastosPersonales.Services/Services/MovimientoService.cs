
using System.Net;
using SeguimientoGastosPersonales.Core.Entities;
using SeguimientoGastosPersonales.Core.Exceptions;
using SeguimientoGastosPersonales.Core.Interfaces;
using SeguimientoGastosPersonales.Core.QueryFilters;
using SeguimientoGastosPersonales.Services.Interfaces;
using SeguimientoGastosPersonales.Core.DTOs;

namespace SeguimientoGastosPersonales.Services.Services
{
    public class MovimientoService : IMovimientoService
    {
        public readonly IUnitOfWork _unitOfWork;

        public MovimientoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Movimiento>> GetAllMovimientosAsync(
            MovimientoQueryFilter? filters = null)
        {
            var movimientos = await _unitOfWork.MovimientoRepository.GetAll();

            if (filters != null)
            {
                if (filters.UsuarioId.HasValue)
                    movimientos = movimientos.Where(m => m.UsuarioId == filters.UsuarioId.Value);

                if (filters.CategoriaId.HasValue)
                    movimientos = movimientos.Where(m => m.CategoriaId == filters.CategoriaId.Value);

                if (!string.IsNullOrWhiteSpace(filters.Tipo))
                    movimientos = movimientos.Where(m =>
                        m.Tipo.Equals(filters.Tipo.Trim(), StringComparison.OrdinalIgnoreCase));

                if (filters.Fecha.HasValue)
                    movimientos = movimientos.Where(m => m.Fecha == filters.Fecha.Value);

                if (!string.IsNullOrWhiteSpace(filters.Descripcion))
                    movimientos = movimientos.Where(m =>
                        m.Descripcion != null &&
                        m.Descripcion.Contains(filters.Descripcion.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            return movimientos;
        }

        public async Task<Movimiento?> GetMovimientoByIdAsync(int id)
        {
            return await _unitOfWork.MovimientoRepository.GetById(id);
        }

        public async Task RegistrarGastoAsync(Movimiento movimiento)
        {
            await RegistrarMovimientoAsync(movimiento, "GASTO");
        }

        public async Task RegistrarIngresoAsync(Movimiento movimiento)
        {
            await RegistrarMovimientoAsync(movimiento, "INGRESO");
        }

        private async Task RegistrarMovimientoAsync(
            Movimiento movimiento,
            string tipoEsperado)
        {
            if (movimiento.Importe <= 0)
                throw new BusinessException(
                    "El importe debe ser mayor que cero.");

            if (movimiento.Fecha == default)
                throw new BusinessException(
                    "La fecha es obligatoria.");

            if (movimiento.Fecha > DateOnly.FromDateTime(DateTime.Today))
                throw new BusinessException(
                    "La fecha no puede ser futura.");

            var usuario =
                await _unitOfWork.UsuarioRepository.GetById(
                    movimiento.UsuarioId);

            if (usuario == null)
                throw new BusinessException(
                    "El usuario indicado no existe.",
                    HttpStatusCode.NotFound);

            var categoria =
                await _unitOfWork.CategoriaRepository.GetById(
                    movimiento.CategoriaId);

            if (categoria == null)
                throw new BusinessException(
                    "La categoría indicada no existe.",
                    HttpStatusCode.NotFound);

            if (categoria.Tipo != tipoEsperado)
                throw new BusinessException(
                    $"La categoría debe ser de tipo {tipoEsperado}.");

            movimiento.Tipo = tipoEsperado;

            await _unitOfWork.MovimientoRepository.Add(movimiento);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ActualizarMovimientoAsync(
            int id,
            Movimiento movimiento)
        {
            var movimientoExistente =
                await _unitOfWork.MovimientoRepository.GetById(id);

            if (movimientoExistente == null)
                return false;

            if (movimiento.Importe <= 0)
                throw new BusinessException(
                    "El importe debe ser mayor que cero.");

            if (movimiento.Fecha == default)
                throw new BusinessException(
                    "La fecha es obligatoria.");

            if (movimiento.Fecha > DateOnly.FromDateTime(DateTime.Today))
                throw new BusinessException(
                    "La fecha no puede ser futura.");

            if (movimiento.Tipo != "INGRESO" &&
                movimiento.Tipo != "GASTO")
                throw new BusinessException(
                    "El tipo debe ser INGRESO o GASTO.");

            var usuario =
                await _unitOfWork.UsuarioRepository.GetById(
                    movimiento.UsuarioId);

            if (usuario == null)
                throw new BusinessException(
                    "El usuario indicado no existe.",
                    HttpStatusCode.NotFound);

            var categoria =
                await _unitOfWork.CategoriaRepository.GetById(
                    movimiento.CategoriaId);

            if (categoria == null)
                throw new BusinessException(
                    "La categoría indicada no existe.",
                    HttpStatusCode.NotFound);

            if (categoria.Tipo != movimiento.Tipo)
                throw new BusinessException(
                    "El tipo de la categoría no coincide con el movimiento.");

            movimientoExistente.UsuarioId = movimiento.UsuarioId;
            movimientoExistente.CategoriaId = movimiento.CategoriaId;
            movimientoExistente.Tipo = movimiento.Tipo;
            movimientoExistente.Descripcion = movimiento.Descripcion;
            movimientoExistente.Importe = movimiento.Importe;
            movimientoExistente.Fecha = movimiento.Fecha;

            await _unitOfWork.MovimientoRepository.Update(
                movimientoExistente);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EliminarMovimientoAsync(int id)
        {
            var movimiento =
                await _unitOfWork.MovimientoRepository.GetById(id);

            if (movimiento == null)
                return false;

            if (movimiento.Id != id)
                throw new BusinessException(
                    "El ID del movimiento no coincide con el ID indicado.");

            await _unitOfWork.MovimientoRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<BalanceDto> ConsultarBalanceAsync(int? usuarioId = null)
        {
            var movimientos = await _unitOfWork.MovimientoRepository.GetAll();

            if (usuarioId.HasValue)
                movimientos = movimientos.Where(m => m.UsuarioId == usuarioId.Value);

            if (!movimientos.Any())
                throw new BusinessException(
                    "No hay información suficiente para calcular el balance.",
                    HttpStatusCode.NotFound);

            var totalIngresos = movimientos
                .Where(m => m.Tipo == "INGRESO")
                .Sum(m => m.Importe);

            var totalGastos = movimientos
                .Where(m => m.Tipo == "GASTO")
                .Sum(m => m.Importe);

            return new BalanceDto
            {
                TotalIngresos = totalIngresos,
                TotalGastos = totalGastos,
                BalanceTotal = totalIngresos - totalGastos
            };
        }
    }
}
