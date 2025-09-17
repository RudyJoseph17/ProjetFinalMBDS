namespace InvestissementsPublics.Starter.Models.Privileges
{
    public class PrivilegeDeleteVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int RolesLinked { get; set; }
        public int UsersLinked { get; set; }
    }
}
