using AutoMapper;
using SeguimientoGastosPersonales.Core.DTOs;
using SeguimientoGastosPersonales.Core.Entities;

namespace SeguimientoGastosPersonales.Infraestructure.Mappings
{
    public class SecurityProfile : Profile
    {
        public SecurityProfile()
        {
            CreateMap<Security, SecurityDto>().ReverseMap();
        }
    }
}
