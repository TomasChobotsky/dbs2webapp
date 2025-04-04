using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Models;

public partial class ChapterContent
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public string? TextFile { get; set; }

    public int ChapterId { get; set; }

    [ForeignKey("ChapterId")]
    public virtual Chapter Chapter { get; set; } = null!;
}
