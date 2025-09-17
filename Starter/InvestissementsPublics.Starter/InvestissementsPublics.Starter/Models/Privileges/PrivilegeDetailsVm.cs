namespace InvestissementsPublics.Starter.Models.Privileges
{
    public class PrivilegeDetailsVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        // Liste des rôles liés
        public List<InvestissementsPublics.Starter.Models.Account.RoleSelection> Roles { get; set; } = new();

        // Liste des utilisateurs liés
        public List<InvestissementsPublics.Starter.Models.Account.UserVm> Users { get; set; } = new();
    }
}
