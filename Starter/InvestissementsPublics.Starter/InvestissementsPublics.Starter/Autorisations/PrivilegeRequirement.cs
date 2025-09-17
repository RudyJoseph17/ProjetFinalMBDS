using Microsoft.AspNetCore.Authorization;

namespace InvestissementsPublics.Starter.Autorisations
{
    public class PrivilegeRequirement: IAuthorizationRequirement
    {
        public string PrivilegeName { get; }
        public PrivilegeRequirement(string privilegeName) => PrivilegeName = privilegeName;
    }
}
