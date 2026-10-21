using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using SeguimientoGastosPersonales.Core.DTOs;
using SeguimientoGastosPersonales.Core.Entities;

namespace SeguimientoGastosPersonales.Infrastructure.Mappings
{
    public class MovimientoProfile : Profile
    {
        public MovimientoProfile()
        {
            CreateMap<Movimiento, MovimientoDto>();
            CreateMap<MovimientoDto, Movimiento>();
        }
    }
}