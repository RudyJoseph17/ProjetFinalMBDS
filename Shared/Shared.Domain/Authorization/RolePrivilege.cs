using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Authorization
{
    public class RolePrivilege
    {
        public string RoleId { get; set; } = null!;     // IdentityRole.Id (string)
        public int PrivilegeId { get; set; }

        // navigations
        public Microsoft.AspNetCore.Identity.IdentityRole? Role { get; set; }
        public Privilege? Privilege { get; set; }
    }
}
