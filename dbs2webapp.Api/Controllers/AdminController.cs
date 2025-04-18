using Application.DTOs;
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
    }
}
