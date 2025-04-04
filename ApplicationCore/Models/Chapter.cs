using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Models;

public partial class Chapter
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Contents { get; set; }

    public int CourseId { get; set; }

    [ForeignKey("CourseId")]
    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual ICollection<ChapterContent> ChapterContents { get; set; } = new List<ChapterContent>();

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
