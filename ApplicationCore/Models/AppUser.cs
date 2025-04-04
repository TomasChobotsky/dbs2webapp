using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ApplicationCore.Models;

public partial class AppUser : IdentityUser
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual ICollection<AssignmentSubmission> AssignmentSubmissions { get; set; } = new List<AssignmentSubmission>();

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual ICollection<AssignmentUser> AssignmentUsers { get; set; } = new List<AssignmentUser>();

    public virtual ICollection<TestInstance> TestInstances { get; set; } = new List<TestInstance>();

    public virtual ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
}
