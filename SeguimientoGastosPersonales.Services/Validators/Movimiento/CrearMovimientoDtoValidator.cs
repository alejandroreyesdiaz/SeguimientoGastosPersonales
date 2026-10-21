using FluentValidation;
using SeguimientoGastosPersonales.Core.DTOs;

namespace SeguimientoGastosPersonales.Services.Validators
{
    public class CrearMovimientoDtoValidator
        : AbstractValidator<MovimientoDto>
    {
        public CrearMovimientoDtoValidator()
        {
            RuleFor(x => x.Id)
                .Equal(0)
                .WithMessage("El ID debe ser cero al crear un movimiento.");

            RuleFor(x => x.UsuarioId)
                .GreaterThan(0)
                .WithMessage("Debe indicar un usuario válido.");

            RuleFor(x => x.CategoriaId)
                .GreaterThan(0)
                .WithMessage("Debe indicar una categoría válida.");

            RuleFor(x => x.Tipo)
                .Must(tipo => tipo == "INGRESO" || tipo == "GASTO")
                .WithMessage("El tipo debe ser INGRESO o GASTO.");

            RuleFor(x => x.Descripcion)
                .MaximumLength(255)
                .WithMessage("La descripción no puede superar los 255 caracteres.");

            RuleFor(x => x.Importe)
                .GreaterThan(0)
                .WithMessage("El importe debe ser mayor que cero.");

            RuleFor(x => x.Fecha)
                .Must(fecha => fecha != default)
                .WithMessage("La fecha es obligatoria.")
                .Must(fecha =>
                    fecha <= DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("La fecha no puede ser futura.");
        }
    }
}