using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbs2webapp.Application.DTOs.Admin
{
    public class AdminCreateUserDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = default!;
        [Required]
        public string Password { get; set; } = default!;
        [Required]
        public string Role { get; set; } = default!;
    }
}
