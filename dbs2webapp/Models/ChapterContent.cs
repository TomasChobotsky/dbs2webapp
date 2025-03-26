using System;
using System.Collections.Generic;

namespace dbs2webapp.Models;

public partial class ChapterContent
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public string? TextFile { get; set; }

    public int ChapterId { get; set; }

    public virtual Chapter Chapter { get; set; } = null!;
}
