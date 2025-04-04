using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Models;

public partial class TestQuestion
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public int TestId { get; set; }

    [ForeignKey("QuestionId")]
    public virtual Question Question { get; set; } = null!;

    [ForeignKey("TestId")]
    public virtual Test Test { get; set; } = null!;
}
