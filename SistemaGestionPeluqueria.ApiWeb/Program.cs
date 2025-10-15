
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using DAL.Repositorios.Interfaces;
using DAL.Repositorios;
using DAL.UnitOfWork;
using DomainLayer.Models;
using DAL.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BLL.Services.Interfaces;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SistemaGestionPeluqueria.ApiWeb.Middlewares;


namespace SistemaGestionPeluqueria.ApiWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Registro de logeo 
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // Add services to the container.        

            //Repositorios y Unidad de trabajo [ALCANCE]:

            //Registro de los ciclos de vida de los Repositorios.
            builder.Services.AddScoped<IAdministradorRepository, AdministradorRepositorio>();
            builder.Services.AddScoped<IClienteRepository, ClienteRepositorio>();
            builder.Services.AddScoped<IEstadoTurnoRepository, EstadoTurnoRepositorio>();
            builder.Services.AddScoped<IHistorialTurnoRepository, HistorialTurnoRepositorio>();
            builder.Services.AddScoped<IMetodoPagoRepository, MetodoPagoRepositorio>();
            builder.Services.AddScoped<ITipoServicioRepository, TipoServicioRepositorio>();
            builder.Services.AddScoped<IServicioRepository, ServicioRepositorio>();
            builder.Services.AddScoped<ITurnoRepository, TurnoRepositorio>();

            //Registro del ciclo de vida de la Unidad de trabajo
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Registro de los ciclos de vida de los Servicios
            builder.Services.AddScoped<IClienteService, ClienteServicio>(); //Cliente
            builder.Services.AddScoped<IMetodoPagoService, MetodoPagoServicio>(); //MetodoPago
            builder.Services.AddScoped<IAdministradorService, AdministradorServicio>(); //Administrador
            builder.Services.AddScoped<IEstadoTurnoService, EstadoTurnoServicio>(); //EstadoTurno
            builder.Services.AddScoped<IHistorialTurnoService, HistorialTurnoServicio>(); //HistorialTurno
            builder.Services.AddScoped<IMetodoPagoService, MetodoPagoServicio>(); //MetodoPago
            builder.Services.AddScoped<IServicioService, ServicioServicio>(); //Servicio
            builder.Services.AddScoped<ITipoServicioService, TipoServicioServicio>(); //TipoServicio
            builder.Services.AddScoped<ITurnoService, TurnoServicio>(); //Turno


            builder.Services.AddControllers();
            //-----------------------------------------------------------------------------

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("PeluqueriaGestion")));


            

            var app = builder.Build();  //CORREGIR EL TEMA DE MODELSTATEWRAPPED YA QUE EL ENSAMBLADO 
                                        // NO LO RECONOCE FUERA DEL MISMO EN LA CAPA DE SERVICIO.
            


            //// Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{

            //}

            app.UseHttpsRedirection();

            app.UseAuthorization();

            //MiddleWares:
            //Customizado
            app.UseMiddleware<CustomLoggingMiddleware>();
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.MapControllers();

            app.MapGet("/", () => Results.Ok("Api de Peluqueria Funciona Correctamente"));

            app.Run();
        }
    }
}
