using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Models;

public partial class AssignmentSubmission
{
    public int Id { get; set; }

    public string? Text { get; set; }

    public string? File { get; set; }

    public int AssignmentId { get; set; }

    public int UserId { get; set; }

    [ForeignKey("AssignmentId")]
    public virtual Assignment Assignment { get; set; } = null!;

    [ForeignKey("UserId")]
    public virtual AppUser User { get; set; } = null!;
}
