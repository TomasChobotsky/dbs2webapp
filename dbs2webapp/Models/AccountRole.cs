using System;
using System.Collections.Generic;

namespace dbs2webapp.Models;

public partial class AccountRole
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AppUser> Users { get; set; } = new List<AppUser>();
}
