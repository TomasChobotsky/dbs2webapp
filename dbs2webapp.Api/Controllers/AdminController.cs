using dbs2webapp.Application.DTOs.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("users")]
        public async Task<ActionResult<List<AdminUserDto>>> GetAllUsers()
        {
            var users = _userManager.Users.ToList();

            var userDtos = new List<AdminUserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new AdminUserDto
                {
                    Id = user.Id,
                    Email = user.Email!,
                    Roles = roles.ToList()
                });
            }

            return Ok(userDtos);
        }

        [HttpGet("users/{id}")]
        public async Task<ActionResult<AdminEditUserDto>> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new AdminEditUserDto
            {
                Email = user.Email!,
                Roles = roles.ToList()
            });
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(AdminCreateUserDto dto)
        {
            var allowedRoles = new[] { "Admin", "Teacher", "Student" };
            if (!allowedRoles.Contains(dto.Role))
                return BadRequest("Invalid role.");

            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing != null)
                return Conflict("User already exists.");

            var user = new IdentityUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, dto.Role);
            return Ok($"User '{dto.Email}' created with role '{dto.Role}'");
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUserRoles(string id, AdminEditUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var existingRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, existingRoles);
            if (!removeResult.Succeeded)
                return BadRequest("Failed to remove old roles.");

            var addResult = await _userManager.AddToRolesAsync(user, dto.Roles);
            if (!addResult.Succeeded)
                return BadRequest("Failed to add new roles.");

            return NoContent();
        }
    }
}
