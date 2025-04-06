using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dbs2webapp.Pages.Admin
{
    [Authorize(Policy = "AdminPolicy")]
    public class EditUserModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditUserModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; } = string.Empty;

        public string UserEmail { get; set; } = string.Empty;
        public List<string> UserRoles { get; set; } = new();
        public List<string> AllRoles { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
                return NotFound();

            UserEmail = user.Email!;
            UserRoles = (await _userManager.GetRolesAsync(user)).ToList();
            AllRoles = _roleManager.Roles.Select(r => r.Name!).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string[] selectedRoles)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
                return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRolesAsync(user, selectedRoles);

            return RedirectToPage("./Users");
        }
    }
}
