using FluentValidation;
using SeguimientoGastosPersonales.Core.DTOs;

namespace SeguimientoGastosPersonales.Services.Validators
{
    public class CrearUsuarioDtoValidator
        : AbstractValidator<CrearUsuarioDto>
    {
        public CrearUsuarioDtoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty()
                .WithMessage("El nombre es obligatorio.")
                .MaximumLength(100)
                .WithMessage("El nombre no puede superar los 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("El correo es obligatorio.")
                .EmailAddress()
                .WithMessage("El correo no tiene un formato válido.")
                .MaximumLength(150)
                .WithMessage("El correo no puede superar los 150 caracteres.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("La contraseña es obligatoria.")
                .MinimumLength(8)
                .WithMessage("La contraseña debe tener al menos 8 caracteres.");
        }
    }
}