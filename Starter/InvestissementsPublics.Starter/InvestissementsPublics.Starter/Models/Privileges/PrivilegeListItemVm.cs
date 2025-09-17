namespace InvestissementsPublics.Starter.Models.Privileges
{
    public class PrivilegeListItemVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int RoleCount { get; set; }
        public int UserDirectCount { get; set; }
    }
}
