using SeguimientoGastosPersonales.Core.Entities;
using SeguimientoGastosPersonales.Core.Exceptions;
using SeguimientoGastosPersonales.Core.Interfaces;
using SeguimientoGastosPersonales.Services.Interfaces;

namespace SeguimientoGastosPersonales.Services.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IBaseRepository<Categoria> _categoriaRepository;
        private readonly IBaseRepository<Movimiento> _movimientoRepository;

        public CategoriaService(
            IBaseRepository<Categoria> categoriaRepository,
            IBaseRepository<Movimiento> movimientoRepository)
        {
            _categoriaRepository = categoriaRepository;
            _movimientoRepository = movimientoRepository;
        }

        public async Task<IEnumerable<Categoria>> GetAllCategoriasAsync()
        {
            return await _categoriaRepository.GetAll();
        }

        public async Task<Categoria?> GetCategoriaByIdAsync(int id)
        {
            if (id <= 0)
                throw new BusinessException(
                    "El ID de la categoría debe ser mayor que cero.");

            return await _categoriaRepository.GetById(id);
        }

        public async Task<Categoria> CrearCategoriaAsync(Categoria categoria)
        {
            ValidarCategoria(categoria.Nombre, categoria.Tipo);

            var categorias = await _categoriaRepository.GetAll();

            if (categorias.Any(c =>
                string.Equals(
                    c.Nombre,
                    categoria.Nombre.Trim(),
                    StringComparison.OrdinalIgnoreCase) &&
                c.Tipo == categoria.Tipo))
            {
                throw new BusinessException(
                    "Ya existe una categoría con ese nombre y tipo.");
            }

            categoria.Nombre = categoria.Nombre.Trim();

            await _categoriaRepository.Add(categoria);

            return categoria;
        }

        public async Task<bool> ActualizarCategoriaAsync(
            int id, Categoria categoria)
        {
            if (id <= 0)
                throw new BusinessException(
                    "El ID debe ser mayor que cero.");

            if (categoria.Id != id)
                throw new BusinessException(
                    "El ID de la categoría no coincide con el ID indicado.");

            ValidarCategoria(categoria.Nombre, categoria.Tipo);

            var categoriaExistente =
                await _categoriaRepository.GetById(id);

            if (categoriaExistente == null)
                return false;

            var categorias = await _categoriaRepository.GetAll();

            if (categorias.Any(c =>
                c.Id != id &&
                string.Equals(
                    c.Nombre,
                    categoria.Nombre.Trim(),
                    StringComparison.OrdinalIgnoreCase) &&
                c.Tipo == categoria.Tipo))
            {
                throw new BusinessException(
                    "Ya existe otra categoría con ese nombre y tipo.");
            }

            categoriaExistente.Nombre = categoria.Nombre.Trim();
            categoriaExistente.Tipo = categoria.Tipo;

            await _categoriaRepository.Update(categoriaExistente);

            return true;
        }

        public async Task<bool> EliminarCategoriaAsync(int id)
        {
            if (id <= 0)
                throw new BusinessException(
                    "El ID de la categoría debe ser mayor que cero.");

            var categoria = await _categoriaRepository.GetById(id);

            if (categoria == null)
                return false;

            var movimientos = await _movimientoRepository.GetAll();

            if (movimientos.Any(m => m.CategoriaId == id))
                throw new BusinessException(
                    "No se puede eliminar la categoría porque tiene movimientos registrados.",
                    System.Net.HttpStatusCode.Conflict);

            await _categoriaRepository.Delete(id);

            return true;
        }

        private static void ValidarCategoria(
            string nombre, string tipo)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new BusinessException(
                    "El nombre de la categoría es obligatorio.");

            if (nombre.Trim().Length > 100)
                throw new BusinessException(
                    "El nombre no puede superar los 100 caracteres.");

            if (tipo != "INGRESO" && tipo != "GASTO")
                throw new BusinessException(
                    "El tipo debe ser INGRESO o GASTO.");
        }
    }
}
