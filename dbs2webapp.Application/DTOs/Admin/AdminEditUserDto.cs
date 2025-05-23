﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbs2webapp.Application.DTOs.Admin
{
    public class AdminEditUserDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = default!;
        public List<string> Roles { get; set; } = new();
    }
}
