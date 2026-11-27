using SeguimientoGastosPersonales.Core.Enums;

namespace SeguimientoGastosPersonales.Core.Entities
{
    public class Security : BaseEntity
    {
        public string User { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public RoleType? Role { get; set; }
    }
}