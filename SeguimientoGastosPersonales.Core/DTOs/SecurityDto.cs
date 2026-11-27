using SeguimientoGastosPersonales.Core.Enums;

namespace SeguimientoGastosPersonales.Core.DTOs
{
    public class SecurityDto
    {
        public string User { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public RoleType? Role { get; set; }
    }
}