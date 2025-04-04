using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Models;

public partial class Course
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public int CourseCategoryId { get; set; }

    [ForeignKey("CourseCategoryId")]
    public virtual CourseCategory CourseCategory { get; set; } = null!;

    public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();

    public virtual ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
}
