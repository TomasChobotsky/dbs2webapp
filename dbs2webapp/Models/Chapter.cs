using System;
using System.Collections.Generic;

namespace dbs2webapp.Models;

public partial class Chapter
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Contents { get; set; }

    public int CourseId { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual ICollection<ChapterContent> ChapterContents { get; set; } = new List<ChapterContent>();

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
