namespace InvestissementsPublics.Starter.Models.Account
{
        public class LoginViewModel
        {
            // L’adresse e-mail ou nom d’utilisateur
            public string UserName { get; set; } = string.Empty;

            // Le mot de passe
            public string Password { get; set; } = string.Empty;

            // Permet de rediriger après connexion
            public string? ReturnUrl { get; set; }
        }
}
