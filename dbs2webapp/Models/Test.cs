using System;
using System.Collections.Generic;

namespace dbs2webapp.Models;

public partial class Test
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int MaxAttempts { get; set; }

    public int ChapterId { get; set; }

    public virtual Chapter Chapter { get; set; } = null!;

    public virtual ICollection<TestInstance> TestInstances { get; set; } = new List<TestInstance>();

    public virtual ICollection<TestQuestion> TestQuestions { get; set; } = new List<TestQuestion>();
}
