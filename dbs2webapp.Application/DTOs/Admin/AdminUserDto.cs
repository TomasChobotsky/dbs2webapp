using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbs2webapp.Application.DTOs.Admin
{
    public class AdminUserDto
    {
        public string Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public List<string> Roles { get; set; } = new();
    }
}