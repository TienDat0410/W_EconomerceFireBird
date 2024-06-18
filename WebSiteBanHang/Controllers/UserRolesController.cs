using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class UserRolesController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }


    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();
        var userRoles = new List<(IdentityUser user, IList<string> roles)>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userRoles.Add((user, roles));
        }

        return View(userRoles);
    }

    public async Task<IActionResult> Manage(string userId)
    {
        ViewBag.userId = userId;
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        var roles = _roleManager.Roles.ToList();
        var userRoles = await _userManager.GetRolesAsync(user);
        var model = (user, roles, userRoles);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Manage(string userId, List<string> selectedRoles)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        var roles = await _userManager.GetRolesAsync(user);
        var result = await _userManager.RemoveFromRolesAsync(user, roles);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Cannot remove user existing roles");
            var rolesList = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            var model = (user, rolesList, userRoles);
            return View(model);
        }

        result = await _userManager.AddToRolesAsync(user, selectedRoles);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Cannot add selected roles to user");
            var rolesList = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            var model = (user, rolesList, userRoles);
            return View(model);
        }

        return RedirectToAction("Index");
    }
}