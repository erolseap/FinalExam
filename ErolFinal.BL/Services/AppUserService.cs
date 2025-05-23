using ErolFinal.BL.Exceptions;
using ErolFinal.DAL.Contexts;

namespace ErolFinal.BL.Services;

public class AppUserService
{
    protected AppDbContext _context;
    protected UserManager<AppUser> _userManager;
    protected SignInManager<AppUser> _signInManager;
    protected RoleManager<AppRole> _roleManager;

	public AppUserService(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
	{
		_context = context;
		_userManager = userManager;
		_signInManager = signInManager;
		_roleManager = roleManager;
	}

	public async Task RegisterAsync(AppUser user, string password)
    {
        await CreateMissingRolesAsync();

        var res = await _userManager.CreateAsync(user, password);
        if (!res.Succeeded)
        {
            throw new RegisterException(res.Errors.ToList());
        }
        await _userManager.AddToRoleAsync(user, "Member");
        await _context.SaveChangesAsync();
    }

    public async Task LoginAsync(string usernameOrEmail, string password)
    {
        await CreateMissingRolesAsync();

        var user = (await _userManager.FindByNameAsync(usernameOrEmail)) ?? (await _userManager.FindByEmailAsync(usernameOrEmail));
        if (user == null)
        {
            throw new LoginException([new IdentityError { Code = "NotFound", Description = "The provided user was not found." }]);
        }
        var res = await _signInManager.PasswordSignInAsync(user, password, false, true);
        if (!res.Succeeded)
        {
            if (res.IsLockedOut)
            {
                throw new LoginException([new IdentityError { Code = "AccountLockedOut", Description = "The provided account is locked out." }]);
            }
            if (res.IsNotAllowed)
            {
                throw new LoginException([new IdentityError { Code = "AccountNotPermitted", Description = "The provided account is disabled/unavailable." }]);
            }
            throw new LoginException([new IdentityError { Code = "WrongPassword", Description = "The provided password is wrong." }]);
        }
    }

    public async Task LogoutAsync() => await _signInManager.SignOutAsync();

    public async Task<AppUser?> GetByNameAsync(string username) => await _userManager.FindByNameAsync(username);

    public async Task MakeAdminAsync(AppUser user)
    {
        await CreateMissingRolesAsync();

        await _userManager.AddToRoleAsync(user, "Admin");
        await _context.SaveChangesAsync();
    }

    public async Task UnmakeAdminAsync(AppUser user)
    {
        await CreateMissingRolesAsync();

        await _userManager.RemoveFromRoleAsync(user, "Admin");
        await _context.SaveChangesAsync();
    }

    protected async Task CreateMissingRolesAsync()
    {
        Func<string, Task> createIfMissing = async name =>
        {
            var role = await _roleManager.FindByNameAsync(name);
            if (role != null) return;
            await _roleManager.CreateAsync(new AppRole() { Name = name });
            await _context.SaveChangesAsync();
        };
        await createIfMissing("Member");
        await createIfMissing("Admin");
    }
}
