using System.ComponentModel.DataAnnotations;

namespace InvestissementsPublics.Starter.Models.Account
{
    public class EditUserViewModel
    {
        [Required] public string Id { get; set; }
        [Required] public string UserName { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required] public string Nom { get; set; }
        [Required] public string Prenom { get; set; }
        [Required] public string Institution { get; set; }
    }
}
