using System.Net;
using SeguimientoGastosPersonales.Core.Entities;
using SeguimientoGastosPersonales.Core.Exceptions;
using SeguimientoGastosPersonales.Core.Interfaces;
using SeguimientoGastosPersonales.Services.Interfaces;

namespace SeguimientoGastosPersonales.Services.Services
{
    public class MovimientoService : IMovimientoService
    {
        public readonly IBaseRepository<Movimiento> _movimientoRepository;
        public readonly IBaseRepository<Usuario> _usuarioRepository;
        public readonly IBaseRepository<Categoria> _categoriaRepository;

        public MovimientoService(
            IBaseRepository<Movimiento> movimientoRepository,
            IBaseRepository<Usuario> usuarioRepository,
            IBaseRepository<Categoria> categoriaRepository)
        {
            _movimientoRepository = movimientoRepository;
            _usuarioRepository = usuarioRepository;
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<Movimiento>> GetAllMovimientosAsync()
        {
            return await _movimientoRepository.GetAll();
        }

        public async Task<Movimiento?> GetMovimientoByIdAsync(int id)
        {
            return await _movimientoRepository.GetById(id);
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
                await _usuarioRepository.GetById(movimiento.UsuarioId);

            if (usuario == null)
                throw new BusinessException(
                    "El usuario indicado no existe.",
                    HttpStatusCode.NotFound);

            var categoria =
                await _categoriaRepository.GetById(movimiento.CategoriaId);

            if (categoria == null)
                throw new BusinessException(
                    "La categoría indicada no existe.",
                    HttpStatusCode.NotFound);

            if (categoria.Tipo != tipoEsperado)
                throw new BusinessException(
                    $"La categoría debe ser de tipo {tipoEsperado}.");

            movimiento.Tipo = tipoEsperado;

            await _movimientoRepository.Add(movimiento);
        }

        public async Task<bool> ActualizarMovimientoAsync(
            int id,
            Movimiento movimiento)
        {
            var movimientoExistente =
                await _movimientoRepository.GetById(id);

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
                await _usuarioRepository.GetById(movimiento.UsuarioId);

            if (usuario == null)
                throw new BusinessException(
                    "El usuario indicado no existe.",
                    HttpStatusCode.NotFound);

            var categoria =
                await _categoriaRepository.GetById(movimiento.CategoriaId);

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

            await _movimientoRepository.Update(movimientoExistente);

            return true;
        }

        public async Task<bool> EliminarMovimientoAsync(int id)
        {
            var movimiento =
                await _movimientoRepository.GetById(id);

            if (movimiento == null)
                return false;

            if (movimiento.Id != id)
                throw new BusinessException(
                    "El ID del movimiento no coincide con el ID indicado.");

            await _movimientoRepository.Delete(id);

            return true;
        }
    }
}
