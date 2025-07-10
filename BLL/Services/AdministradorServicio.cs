using BLL.Services.Interfaces;
using DAL.Repositorios.Interfaces;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BLL.BCryptHasher;

namespace BLL.Services
{
    public class AdministradorServicio : IAdministradorService
    {
        public readonly IAdministradorRepository _administradorRepository;

        public AdministradorServicio(IAdministradorRepository administradorRepository)
        {
            _administradorRepository = administradorRepository;
        }

        public async Task<IEnumerable<Administrador>> ObtenerTodos()
        {
            return await _administradorRepository.GetAllAsync();
        }

        public async Task<Administrador?> ObtenerPorId(int id)
        {
            return await _administradorRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Se encarga de Crear un Administrador con los datos de: Usuario, Correo y Contrasena(Ver Modelo Administrador)
        /// </summary>
        /// <param name="admin">Ingresa por Formulario</param>
        /// <returns>  Un tipo OperationResult  </returns>
        public async Task<OperationResult<Administrador>> Crear(Administrador admin)
        {
            //El metodo ValidarAdministrador retorna un tipo OperationResult...
            var validarAdmin = ValidarAdministrador(admin);
            if (!validarAdmin.Success)
                 return validarAdmin;

            //Aca se debe hashear la contrasenia
            admin.Contrasena = PasswordHasher.Hashear(admin.Contrasena);

            await _administradorRepository.AddAsync(admin);

            return OperationResult<Administrador>.Ok(admin);
        }

        public async Task<OperationResult<Administrador>> Actualizar(Administrador admin, int id)
        {

            var adminExiste = await _administradorRepository.BuscarAsync(id);
            if (adminExiste == null)
            {
                return OperationResult<Administrador>.Fail("El registro no existe en la base de datos.");
            }

            var adminValidar = ValidarAdministrador(admin);
            if (!adminValidar.Success)//Si entra en esta condicion el resultado de la validacion
            {                         // significa que algo salio mal
                return adminValidar; //Aca retorna el tipo OoperationResilt<Admin> pero con una lista de errores
            }                        //y la prop. success => false.

            //Asignacion de los valores del parametro de entrada 'admin' al objeto adminExiste para aplicar
            //los cambios.
            adminExiste.Correo = admin.Correo;
            adminExiste.Usuario = admin.Usuario;
            //Comprueba si la contrasenia ah sido actualizada tambien...
            if(admin.Contrasena != adminExiste.Contrasena) adminExiste.Contrasena = PasswordHasher.Hashear(admin.Contrasena);

            await _administradorRepository.UpdateAsync(adminExiste);

            return OperationResult<Administrador>.Ok(adminExiste);
        }

       

        public async Task<OperationResult<bool>> Eliminar(int id)
        {
            var administradorExiste = await _administradorRepository.VerificarSiExiste(id);
            if (!administradorExiste)//Si no Existe enviar un mensaje de Error
            {
                return OperationResult<bool>.Fail("El id del Administrador no se encuentra en los registros!");
            }

            //Si Existe el registro, lo elimina
            //Y retorna un tipo OperationResult<bool> con las propiedades {Success= True, Data= True}.
            await _administradorRepository.Delete(id);
            return OperationResult<bool>.Ok(true);
        }

        
        /// <summary>
        /// Metodo de Inicio de Sesion para un Administrador.
        /// </summary>
        /// <param name="correo">correo o usuario Guardados en la base de datos del Administrador</param>
        /// <param name="contrasenia">Contrasenia hasheada en la base de datos</param>
        /// <returns> Un OperationResult con dos Valores: Success=True, Data= Administrador </returns>
        public async Task<OperationResult<Administrador>> IniciarSesion(string correo, string contrasenia)
        {   
            //1-A) Busca el objeto de Administrador a traves del correo ingresado
            var admin = await _administradorRepository.ObtenerPorUsuarioCorreoAsync(correo);

            //1-B) Si el objeto no se halla por su correo entontes no existe en la base de datos.
            if(admin == null)
            {
                return OperationResult<Administrador>.Fail("Correo Invalido");
            }

            //1-C) Si encuentra el Usuario entonces verificamos la contrasenia
            //Si el metodo Verificar retorna False encontes retornamos "contrasenia incorrecta"
            if(!PasswordHasher.Verificar(contrasenia, admin.Contrasena))
            {
                return OperationResult<Administrador>.Fail("Contrasena Invalida");
            }

            return OperationResult<Administrador>.Ok(admin);

        }


        //Validar Administrador
        /// <summary>
        /// VALIDA CADA CAMPO DEL MODELO DE ADMNISTRADOR
        /// </summary>
        /// <param name="administrador"></param>
        /// <returns></returns>
        public OperationResult<Administrador> ValidarAdministrador(Administrador administrador)
        {
            var errores = new List<string>();

            //INICIO DE VALIDACIONES

            if (string.IsNullOrWhiteSpace(administrador.Usuario)) errores.Add("El nombre de Usuario es requerido");

            if (string.IsNullOrWhiteSpace(administrador.Correo)) errores.Add("El correo es requerido");
            else if (!Regex.IsMatch(administrador.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errores.Add("El formato del Correo Electronico es invalido");

            if (string.IsNullOrWhiteSpace(administrador.Contrasena)) errores.Add("La contrasenia es requerida");
            else if (administrador.Contrasena.Length < 6) errores.Add("La contrasenia debe tener al menos 6 caracteres");

            //FIN DE INICIO DE VALIDACIONES

            if (errores.Any()) return OperationResult<Administrador>.Fail(errores.ToArray());

                //Si no hay errores en la validacion de los datos para crear un administrador
                //se retorna un tipo OperationResult que admite tipo 'Administrador' y 
                return OperationResult<Administrador>.Ok(administrador);
        }
    }
}
