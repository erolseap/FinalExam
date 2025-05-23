using System.ComponentModel.DataAnnotations;

namespace ErolFinal.MVC.ViewModels;

public class AccountRegisterVM
{
    [Required]
    public string Username { get; set; }

	[Required]
    [MinLength(3), MaxLength(95 + 1)]
	public string Name { get; set; }

	[Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
