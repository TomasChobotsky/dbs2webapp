using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Models;

public partial class Option
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public bool IsCorrect { get; set; }

    public int QuestionId { get; set; }

    [ForeignKey("QuestionId")]
    public virtual Question Question { get; set; } = null!;
}
