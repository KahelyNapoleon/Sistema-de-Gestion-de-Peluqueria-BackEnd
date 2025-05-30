
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


namespace SistemaGestionPeluqueria.ApiWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

          

            //Repositorios y Unidad de trabajo [ALCANCE]:

            builder.Services.AddScoped<IAdministradorRepository, AdministradorRepositorio>();
            builder.Services.AddScoped<IClienteRepository, ClienteRepositorio>();
            builder.Services.AddScoped<IEstadoTurnoRepository, EstadoTurnoRepositorio>();
            builder.Services.AddScoped<IHistorialTurnoRepository, HistorialTurnoRepositorio>();
            builder.Services.AddScoped<IMetodoPagoRepository, MetodoPagoRepositorio>();
            builder.Services.AddScoped<ITipoServicioRepository, TipoServicioRepositorio>();
            builder.Services.AddScoped<IServicioRepository, ServicioRepositorio>();
            builder.Services.AddScoped<ITurnoRepository, TurnoRepositorio>();

            //Unidad de trabajo
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Servicios
            builder.Services.AddScoped<IMetodoPagoService, MetodoPagoServicio>(); //MetodoPago
            //Model StateDictionary
            builder.Services.AddScoped<IValidationDictionary, OperationResult>();

            //-----------------------------------------------------------------------------

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("PeluqueriaGestion")));


            builder.Services.AddControllers();

            var app = builder.Build();  //CORREGIR EL TEMA DE MODELSTATEWRAPPED YA QYE EL ENSAMBLADO 
            // NO LO RECONOCE FUERA DEL MISMO EN LA CAPA DE SERVICIO.

            //// Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
               
            //}

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapGet("/", () => Results.Ok("Api de Peluqueria Funciona Correctamente"));

            app.Run();
        }
    }
}
