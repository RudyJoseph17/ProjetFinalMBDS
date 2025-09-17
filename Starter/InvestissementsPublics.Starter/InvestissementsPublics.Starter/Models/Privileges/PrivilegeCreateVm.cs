using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace InvestissementsPublics.Starter.Models.Privileges
{
    public class PrivilegeCreateVm
    {
        public int? Id { get; set; }

        [Required, StringLength(200)]
        [Display(Name = "Nom du privilège (ex: BanqueProjet.Projet.Create)")]
        public string Name { get; set; } = null!;

        [StringLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        // Sélections
        [Display(Name = "Rôles (cocher ceux qui auront ce privilège)")]
        public List<string> SelectedRoleIds { get; set; } = new List<string>();

        [Display(Name = "Utilisateurs (attribution directe via claim)")]
        public List<string> SelectedUserIds { get; set; } = new List<string>();

        // Listes pour la vue
        public List<SelectListItem> AvailableRoles { get; set; } = new();
        public List<SelectListItem> AvailableUsers { get; set; } = new();
    }
}
