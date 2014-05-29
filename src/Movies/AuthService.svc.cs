using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Movies.DataContracts;
using System.ServiceModel.Activation;

namespace Movies
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] 
    public class AuthService : IAuthService
    {
      
        public AuthResult Authenticate(DataContracts.AuthData userInfo)
        {
            if (userInfo.Login == "admin" && userInfo.Password == "1234") {
                return new AuthResult() { Token=GenerateToken(userInfo.Login, userInfo.Password), Name="admin", Login="admin"};
            }
            throw new UnauthorizedAccessException();
        }

        private static string GenerateToken(string name, string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(name + ":" + password));
        }
    }
}
