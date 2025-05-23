using Microsoft.AspNetCore.Identity;

namespace ErolFinal.DAL.Models;

public class AppUser : IdentityUser
{
    public string Name { get; set; }
}
