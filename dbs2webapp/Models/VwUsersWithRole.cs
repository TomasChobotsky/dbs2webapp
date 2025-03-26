using System;
using System.Collections.Generic;

namespace dbs2webapp.Models;

public partial class VwUsersWithRole
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string RoleName { get; set; } = null!;
}
