using System;
using System.Collections.Generic;

namespace dbs2webapp.Models;

public partial class VwAssignmentSubmission
{
    public int SubmissionId { get; set; }

    public string? SubmissionText { get; set; }

    public string? SubmissionFile { get; set; }

    public string AssignmentTitle { get; set; } = null!;

    public int UserId { get; set; }

    public string Firstname { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;
}
