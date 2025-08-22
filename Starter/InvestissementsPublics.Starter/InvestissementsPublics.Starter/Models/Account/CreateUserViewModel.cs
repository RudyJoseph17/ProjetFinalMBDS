using System.ComponentModel.DataAnnotations;

namespace InvestissementsPublics.Starter.Models.Account
{
    public class CreateUserViewModel
    {
        [Required] public string Id { get; set; }
        [Required] public string UserName { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        //[Required] public string Nom { get; set; }
        //[Required] public string Prenom { get; set; }
        //[Required] public string Institution { get; set; }
        [Required, DataType(DataType.Password)] public string Password { get; set; }
        [DataType(DataType.Password), Compare("Password")] public string ConfirmPassword { get; set; }
    }
}
