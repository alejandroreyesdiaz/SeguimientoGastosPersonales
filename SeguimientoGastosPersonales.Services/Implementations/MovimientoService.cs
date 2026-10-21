
using AutoMapper;
using SeguimientoGastosPersonales.Core.DTOs;
using SeguimientoGastosPersonales.Core.Entities;
using SeguimientoGastosPersonales.Core.Interfaces;
using SeguimientoGastosPersonales.Services.Interfaces;

namespace SeguimientoGastosPersonales.Services.Implementations
{
    public class MovimientoService : IMovimientoService
    {
        private readonly IMovimientoRepository _movimientoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public MovimientoService(
            IMovimientoRepository movimientoRepository,
            IUsuarioRepository usuarioRepository,
            ICategoriaRepository categoriaRepository,
            IMapper mapper)
        {
            _movimientoRepository = movimientoRepository;
            _usuarioRepository = usuarioRepository;
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MovimientoDto>>
            GetAllMovimientosAsync()
        {
            var movimientos =
                await _movimientoRepository.GetAllMovimientosAsync();

            return _mapper.Map<IEnumerable<MovimientoDto>>(movimientos);
        }

        public async Task<MovimientoDto?> GetMovimientoByIdAsync(int id)
        {
            var movimiento =
                await _movimientoRepository.GetMovimientosByIdAsync(id);

            if (movimiento == null)
                return null;

            return _mapper.Map<MovimientoDto>(movimiento);
        }

        public async Task<MovimientoDto> RegistrarGastoAsync(
            CrearGastoDto dto)
        {
            return await RegistrarMovimientoAsync(
                dto.UsuarioId,
                dto.CategoriaId,
                dto.Descripcion,
                dto.Importe,
                dto.Fecha,
                "GASTO");
        }

        public async Task<MovimientoDto> RegistrarIngresoAsync(
            CrearIngresoDto dto)
        {
            return await RegistrarMovimientoAsync(
                dto.UsuarioId,
                dto.CategoriaId,
                dto.Descripcion,
                dto.Importe,
                dto.Fecha,
                "INGRESO");
        }

        private async Task<MovimientoDto> RegistrarMovimientoAsync(
            int usuarioId,
            int categoriaId,
            string? descripcion,
            decimal importe,
            DateOnly fecha,
            string tipoEsperado)
        {
            if (importe <= 0)
                throw new ArgumentException(
                    "El importe debe ser mayor que cero.");

            if (fecha == default)
                throw new ArgumentException(
                    "La fecha es obligatoria.");

            if (fecha > DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException(
                    "La fecha no puede ser futura.");

            var usuario =
                await _usuarioRepository.GetUsuariosByIdAsync(usuarioId);

            if (usuario == null)
                throw new InvalidOperationException(
                    "El usuario indicado no existe.");

            var categoria =
                await _categoriaRepository.GetCategoriasByIdAsync(
                    categoriaId);

            if (categoria == null)
                throw new InvalidOperationException(
                    "La categoría indicada no existe.");

            if (categoria.Tipo != tipoEsperado)
                throw new InvalidOperationException(
                    $"La categoría debe ser de tipo {tipoEsperado}.");

            var movimiento = new Movimiento
            {
                UsuarioId = usuarioId,
                CategoriaId = categoriaId,
                Tipo = tipoEsperado,
                Descripcion = descripcion,
                Importe = importe,
                Fecha = fecha
            };

            await _movimientoRepository.InsertMovimiento(movimiento);

            return _mapper.Map<MovimientoDto>(movimiento);
        }

        public async Task<bool> ActualizarMovimientoAsync(
            int id, ActualizarMovimientoDto dto)
        {
            var movimiento =
                await _movimientoRepository.GetMovimientosByIdAsync(id);

            if (movimiento == null)
                return false;

            if (dto.Importe <= 0)
                throw new ArgumentException(
                    "El importe debe ser mayor que cero.");

            if (dto.Fecha == default)
                throw new ArgumentException(
                    "La fecha es obligatoria.");

            if (dto.Fecha > DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException(
                    "La fecha no puede ser futura.");

            if (dto.Tipo != "INGRESO" && dto.Tipo != "GASTO")
                throw new ArgumentException(
                    "El tipo debe ser INGRESO o GASTO.");

            var usuario =
                await _usuarioRepository.GetUsuariosByIdAsync(dto.UsuarioId);

            if (usuario == null)
                throw new InvalidOperationException(
                    "El usuario indicado no existe.");

            var categoria =
                await _categoriaRepository.GetCategoriasByIdAsync(
                    dto.CategoriaId);

            if (categoria == null)
                throw new InvalidOperationException(
                    "La categoría indicada no existe.");

            if (categoria.Tipo != dto.Tipo)
                throw new InvalidOperationException(
                    "El tipo de la categoría no coincide con el movimiento.");

            movimiento.UsuarioId = dto.UsuarioId;
            movimiento.CategoriaId = dto.CategoriaId;
            movimiento.Tipo = dto.Tipo;
            movimiento.Descripcion = dto.Descripcion;
            movimiento.Importe = dto.Importe;
            movimiento.Fecha = dto.Fecha;

            await _movimientoRepository.UpdateMovimiento(movimiento);

            return true;
        }

        public async Task<bool> EliminarMovimientoAsync(int id)
        {
            var movimiento =
                await _movimientoRepository.GetMovimientosByIdAsync(id);

            if (movimiento == null)
                return false;

            await _movimientoRepository.DeleteMovimiento(movimiento);

            return true;
        }
    }
}
