using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace BLL.BCryptHasher
{
    public static class PasswordHasher
    {
        public static string Hashear(string contrasenia)
        {
            return BCrypt.Net.BCrypt.HashPassword(contrasenia);
        }

        public static bool Verificar(string inputPassword, string saveHash)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, saveHash);
        }
    }
}
