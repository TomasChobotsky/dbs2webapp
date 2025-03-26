using System;
using System.Collections.Generic;

namespace dbs2webapp.Models;

public partial class VwCourseDetail
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public int? ChapterId { get; set; }

    public string? ChapterName { get; set; }

    public int? TestId { get; set; }

    public string? TestTitle { get; set; }

    public int? MaxAttempts { get; set; }
}
