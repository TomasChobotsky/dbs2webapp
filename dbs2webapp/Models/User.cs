using System;
using System.Collections.Generic;

namespace dbs2webapp.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<AssignmentSubmission> AssignmentSubmissions { get; set; } = new List<AssignmentSubmission>();

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual ICollection<AssignmentUser> AssignmentUsers { get; set; } = new List<AssignmentUser>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<TestInstance> TestInstances { get; set; } = new List<TestInstance>();

    public virtual ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
}
