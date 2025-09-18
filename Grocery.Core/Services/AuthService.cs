using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClientService _clientService;

        public AuthService(IClientService clientService)
        {
            _clientService = clientService;
        }

        public Client? Login(string email, string password)
        {
            
            var client = _clientService.Get(email);
            //opvragen van de klantgegevens (client) met het ingevoerde email adres.

            if (client == null)//controleert of de client bestaat in het systeem.
            {
                return null;
            }

            
            bool isPasswordValid = PasswordHelper.VerifyPassword(password, client.Password);
            //als er een client is gevonden wordt er gecontroleerd of het opgegeven wachtwoord daarbij matcht.

            
            if (isPasswordValid) //returnt klantgegevens als het klopt, anders is de return null.
            {
                return client;
            }

            return null;
        }
    }
}