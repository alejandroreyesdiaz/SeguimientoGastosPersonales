
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SeguimientoGastosPersonales.Api.Filters;
using SeguimientoGastosPersonales.Core.Entities;
using SeguimientoGastosPersonales.Core.Interfaces;
using SeguimientoGastosPersonales.Infrastructure.Data;
using SeguimientoGastosPersonales.Infrastructure.Repositories;
using SeguimientoGastosPersonales.Services.Interfaces;
using SeguimientoGastosPersonales.Services.Services;
using SeguimientoGastosPersonales.Services.Validators;
using SeguimientoGastosPersonales.Core.CustomEntities;
using System.Text;

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

            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            builder.Services.AddScoped<IDapperContext, DapperContext>();

            builder.Services.AddScoped<ISecurityService, SecurityService>();
            builder.Services.AddScoped<IReportesService, ReportesService>();
            builder.Services.AddScoped<IReportesRepository, ReportesRepository>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Authentication:Issuer"],
                    ValidAudience = builder.Configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]!))
                };
            });

            builder.Services.Configure<SeguimientoGastosPersonales.Core.CustomEntities.PasswordOptions>(
            builder.Configuration.GetSection("PasswordOptions"));

            builder.Services.AddSingleton<IPasswordService, PasswordService>();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
