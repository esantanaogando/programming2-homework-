using Impresiones3D.API.Exceptions;
using Impresiones3D.Application.Services;
using Impresiones3D.Domain.Interfaces;
using Impresiones3D.Infrastructure.UnitOfWork;
using Impresiones3D.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Impresiones3D.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Base de datos
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Inyección de dependencias
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ClienteService>();
            builder.Services.AddScoped<OrdenService>();
            builder.Services.AddScoped<MaterialService>();
            builder.Services.AddScoped<ImpresoraService>();
            builder.Services.AddScoped<ReporteService>();
            builder.Services.AddScoped<ConfiguracionService>();

            builder.Services.AddControllers();

            // NUEVO: Swagger UI para probar endpoints visualmente
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new()
                {
                    Title = "Impresiones3D API",
                    Version = "v1",
                    Description = "API REST para gestión de órdenes de impresión 3D — 3DPrintFlow"
                });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowWeb", policy =>
                {
                    policy.WithOrigins("https://localhost:7208")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // GlobalExceptionHandler como middleware (captura todo lo no controlado)
            app.UseMiddleware<GlobalExceptionHandler>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Impresiones3D API v1");
                    c.RoutePrefix = "swagger"; // accede en /swagger
                });
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowWeb");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}