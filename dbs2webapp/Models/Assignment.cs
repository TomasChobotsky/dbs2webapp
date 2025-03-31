using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbs2webapp.Models;

public partial class Assignment
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime DueDate { get; set; }

    public int TeacherId { get; set; }
    [ForeignKey("TeacherId")]
    public virtual User? Teacher { get; set; }

    public int ChapterId { get; set; }
    [ForeignKey("ChapterId")]
    public virtual Chapter? Chapter { get; set; }

    public virtual ICollection<AssignmentSubmission> AssignmentSubmissions { get; set; } = new List<AssignmentSubmission>();

    public virtual ICollection<AssignmentUser> AssignmentUsers { get; set; } = new List<AssignmentUser>();
}
