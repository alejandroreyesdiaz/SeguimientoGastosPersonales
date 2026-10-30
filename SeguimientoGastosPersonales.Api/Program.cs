
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SeguimientoGastosPersonales.Core.Entities;
using SeguimientoGastosPersonales.Core.Interfaces;
using SeguimientoGastosPersonales.Infrastructure.Repositories;
using SeguimientoGastosPersonales.Infrastructure.Data;
using SeguimientoGastosPersonales.Services.Services;
using SeguimientoGastosPersonales.Services.Interfaces;
using FluentValidation;
using SeguimientoGastosPersonales.Services.Validators;
using SeguimientoGastosPersonales.Api.Filters;

namespace SeguimientoGastosPersonales.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SeguimientoGastosPersonalesContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(
                        builder.Configuration.GetConnectionString("DefaultConnection")
                    )
                )
            );

            builder.Services.AddAutoMapper(
                AppDomain.CurrentDomain.GetAssemblies()
            );

            // Add services to the container.
            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            //builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            //builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            //builder.Services.AddScoped<IMovimientoRepository, MovimientoRepository>();

            builder.Services.AddScoped(
                typeof(IBaseRepository<>),
                typeof(BaseRepository<>));

            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
            builder.Services.AddScoped<ICategoriaService, CategoriaService>();
            builder.Services.AddScoped<IMovimientoService, MovimientoService>();

            builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

            // Registrar validadores de FluentValidation
            builder.Services.AddValidatorsFromAssemblyContaining<CrearUsuarioDtoValidator>();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
