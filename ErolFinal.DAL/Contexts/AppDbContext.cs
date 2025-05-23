using ErolFinal.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ErolFinal.DAL.Contexts;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public DbSet<TrainerModel> Trainers { get; set; }

    public AppDbContext(DbContextOptions opts) : base(opts)
    {
    }
}
