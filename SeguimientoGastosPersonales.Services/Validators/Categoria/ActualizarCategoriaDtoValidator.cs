
using FluentValidation;
using SeguimientoGastosPersonales.Core.DTOs;

namespace SeguimientoGastosPersonales.Services.Validators
{
    public class ActualizarCategoriaDtoValidator
        : AbstractValidator<ActualizarCategoriaDto>
    {
        public ActualizarCategoriaDtoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty()
                .WithMessage("El nombre es obligatorio.")
                .MaximumLength(100)
                .WithMessage(
                    "El nombre no puede superar los 100 caracteres.");

            RuleFor(x => x.Tipo)
                .Must(tipo =>
                    tipo == "INGRESO" || tipo == "GASTO")
                .WithMessage(
                    "El tipo debe ser INGRESO o GASTO.");
        }
    }
}
