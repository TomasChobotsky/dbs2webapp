using System;
using System.Collections.Generic;

namespace ApplicationCore.Models;

public partial class Question
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public virtual ICollection<Option> Options { get; set; } = new List<Option>();

    public virtual ICollection<TestQuestion> TestQuestions { get; set; } = new List<TestQuestion>();
}
