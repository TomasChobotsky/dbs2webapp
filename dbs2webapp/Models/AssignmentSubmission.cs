using System;
using System.Collections.Generic;

namespace dbs2webapp.Models;

public partial class AssignmentSubmission
{
    public int Id { get; set; }

    public string? Text { get; set; }

    public string? File { get; set; }

    public int AssignmentId { get; set; }

    public int UserId { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
