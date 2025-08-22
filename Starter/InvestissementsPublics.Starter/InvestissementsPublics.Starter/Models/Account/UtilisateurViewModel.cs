using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace InvestissementsPublics.Starter.Models.Account
{
    public class UtilisateurViewModel
    {
        public string Id { get; set; } = "";

        [Required]
        public string UserName { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        public string Nom { get; set; } = "";
        public string Prenom { get; set; } = "";
        public string Institution { get; set; } = "";

        [Required, DataType(DataType.Password), StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = "";

        //[Display(Name = "Rôle")]
        [Required(ErrorMessage = "Vous devez choisir un rôle")]
        public string SelectedRole { get; set; } = "";

        // La liste des rôles disponibles pour le dropdown
        public List<SelectListItem> AvailableRoles { get; set; } = new();
    }

}
