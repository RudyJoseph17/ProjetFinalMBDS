using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Authorization
{
    public class Privilege
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;   // ex: "BanqueProjet.Projet.Create"

        [MaxLength(500)]
        public string? Description { get; set; }

        public ICollection<RolePrivilege> RolePrivileges { get; set; } = new List<RolePrivilege>();
    }
}
