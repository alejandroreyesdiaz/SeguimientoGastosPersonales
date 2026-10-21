
using FluentValidation;
using SeguimientoGastosPersonales.Core.DTOs;

namespace SeguimientoGastosPersonales.Services.Validators
{
    public class CrearIngresoDtoValidator
        : AbstractValidator<CrearIngresoDto>
    {
        public CrearIngresoDtoValidator()
        {
            RuleFor(x => x.UsuarioId)
                .GreaterThan(0)
                .WithMessage("El usuario es obligatorio.");

            RuleFor(x => x.CategoriaId)
                .GreaterThan(0)
                .WithMessage("La categoría es obligatoria.");

            RuleFor(x => x.Descripcion)
                .MaximumLength(255)
                .WithMessage(
                    "La descripción no puede superar los 255 caracteres.");

            RuleFor(x => x.Importe)
                .GreaterThan(0)
                .WithMessage("El importe debe ser mayor que cero.");

            RuleFor(x => x.Fecha)
                .Must(fecha => fecha != default)
                .WithMessage("La fecha es obligatoria.");

            RuleFor(x => x.Fecha)
                .Must(fecha =>
                    fecha == default ||
                    fecha <= DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("La fecha no puede ser futura.");
        }
    }
}
