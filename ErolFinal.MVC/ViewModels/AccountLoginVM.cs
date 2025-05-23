using System.ComponentModel.DataAnnotations;

namespace ErolFinal.MVC.ViewModels;

public class AccountLoginVM
{
    [Required]
    public string UsernameOrEmail { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
