using System;
using System.Collections.Generic;

namespace dbs2webapp.Models;

public partial class UserCourse
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual AppUser User { get; set; } = null!;
}
