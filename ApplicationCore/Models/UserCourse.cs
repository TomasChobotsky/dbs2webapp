using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Models;

public partial class UserCourse
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CourseId { get; set; }

    [ForeignKey("CourseId")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("UserId")]
    public virtual AppUser User { get; set; } = null!;
}
