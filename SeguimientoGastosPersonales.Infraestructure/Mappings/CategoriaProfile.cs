using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using SeguimientoGastosPersonales.Core.DTOs;
using SeguimientoGastosPersonales.Core.Entities;

namespace SeguimientoGastosPersonales.Infrastructure.Mappings
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
            CreateMap<Categoria, CategoriaDto>();

            CreateMap<CategoriaDto, Categoria>();

            CreateMap<CrearCategoriaDto, Categoria>();

            CreateMap<ActualizarCategoriaDto, Categoria>();
        }
    }
}