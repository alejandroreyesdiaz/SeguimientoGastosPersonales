using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SeguimientoGastosPersonales.Core.Entities;
using SeguimientoGastosPersonales.Core.Interfaces;
using SeguimientoGastosPersonales.Infrastructure.Repositories;
using SeguimientoGastosPersonales.Infrastructure.Data;
using SeguimientoGastosPersonales.Services.Implementations;
using SeguimientoGastosPersonales.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using SeguimientoGastosPersonales.Services.Validators;

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

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            builder.Services.AddScoped<IMovimientoRepository, MovimientoRepository>();

            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
            builder.Services.AddScoped<ICategoriaService, CategoriaService>();
            builder.Services.AddScoped<IMovimientoService, MovimientoService>();

            builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddValidatorsFromAssemblyContaining<
                CrearUsuarioDtoValidator>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
