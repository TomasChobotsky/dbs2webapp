using System;
using System.Collections.Generic;

namespace dbs2webapp.Models;

public partial class TestQuestion
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public int TestId { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual Test Test { get; set; } = null!;
}
