
using AutoMapper;
using SeguimientoGastosPersonales.Core.DTOs;
using SeguimientoGastosPersonales.Core.Entities;
using SeguimientoGastosPersonales.Core.Interfaces;
using SeguimientoGastosPersonales.Services.Interfaces;

namespace SeguimientoGastosPersonales.Services.Implementations
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public CategoriaService(
            ICategoriaRepository categoriaRepository,
            IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoriaDto>>
            GetAllCategoriasAsync()
        {
            var categorias =
                await _categoriaRepository.GetAllCategoriasAsync();

            return _mapper.Map<IEnumerable<CategoriaDto>>(categorias);
        }

        public async Task<CategoriaDto?> GetCategoriaByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID de la categoría debe ser mayor que cero.");

            var categoria =
                await _categoriaRepository.GetCategoriasByIdAsync(id);

            if (categoria == null)
                return null;

            return _mapper.Map<CategoriaDto>(categoria);
        }

        public async Task<CategoriaDto> CrearCategoriaAsync(
    CrearCategoriaDto dto)
        {
            ValidarCategoria(dto.Nombre, dto.Tipo);

            var categoria = new Categoria
            {
                Nombre = dto.Nombre.Trim(),
                Tipo = dto.Tipo
            };

            await _categoriaRepository.InsertCategoria(categoria);

            return _mapper.Map<CategoriaDto>(categoria);
        }

        public async Task<bool> ActualizarCategoriaAsync(
    int id, ActualizarCategoriaDto dto)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID debe ser mayor que cero.");

            ValidarCategoria(dto.Nombre, dto.Tipo);

            var categoria =
                await _categoriaRepository.GetCategoriasByIdAsync(id);

            if (categoria == null)
                return false;

            categoria.Nombre = dto.Nombre.Trim();
            categoria.Tipo = dto.Tipo;

            await _categoriaRepository.UpdateCategoria(categoria);

            return true;
        }

        public async Task<bool> EliminarCategoriaAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID de la categoría debe ser mayor que cero.");

            var categoria =
                await _categoriaRepository.GetCategoriasByIdAsync(id);

            if (categoria == null)
                return false;

            await _categoriaRepository.DeleteCategoria(categoria);

            return true;
        }

        private static void ValidarCategoria(
    string nombre, string tipo)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException(
                    "El nombre de la categoría es obligatorio.");

            if (nombre.Trim().Length > 100)
                throw new ArgumentException(
                    "El nombre no puede superar los 100 caracteres.");

            if (tipo != "INGRESO" && tipo != "GASTO")
                throw new ArgumentException(
                    "El tipo debe ser INGRESO o GASTO.");
        }
    }
}

