
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BOMComparator.Models
{
    public class AuthenticationService : IAuthenticationService
    {
        private class InternalUserData
        {
            public InternalUserData(string email, string[] roles, string hashedPassword)
            {
                Email = email;
                Roles = roles;
                HashedPassword = hashedPassword;
            }
            public string Email { get; private set; }
            public string[] Roles { get; private set; }
            public string HashedPassword { get; private set; }
        }

        private static readonly List<InternalUserData> _users = new List<InternalUserData>()
        {
            new InternalUserData ("adszulc@danfoss.com", new string[] {"Administrators", "SuperUsers", "Users"},"MB5PYIsbI2YzCUe34Q5ZU2VferIoI4Ttd+ydolWV0OE="),
            new InternalUserData ("newGuy@danfoss.com", new string[] {"Users"},"hMaLizwzOQ5LeOnMuj+C6W75Zl5CXXYbwDSHWW9ZOXc=")
        };

        public User AuthenticateUser(string username, string password)
        {
            /*InternalUserData userData = _users.FirstOrDefault(u => u.Equals(username) && u.HashedPassword.Equals(CalculateHash(clearTextPassword, u.Username)));
            if (userData == null)
            {
                throw new UnauthorizedAccessException("Access denided. Password or email is wrong.");
            }*/
            throw new NotImplementedException();
        }
    }
}
