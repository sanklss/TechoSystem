using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnoSystem.Data;
using TechnoSystem.Models;

namespace TechnoSystem.Services
{
    internal class AuthService
    {
        public Role TryAuth(string login, string password)
        {
            using (var context = new Primer21Context())
            {
                User user = context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

                if (user == null)
                {
                    return null;
                }

                return context.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            }
        }
    }
}
