namespace InvestissementsPublics.Starter.Models.Account
{
    public class UtilisateurListeViewModel
    {
        public string Id { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public List<string> Roles { get; set; } = new();
    }
}
