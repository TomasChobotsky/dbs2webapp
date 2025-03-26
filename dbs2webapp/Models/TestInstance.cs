using System;
using System.Collections.Generic;

namespace dbs2webapp.Models;

public partial class TestInstance
{
    public int Id { get; set; }

    public DateTime StartedAt { get; set; }

    public int Attempt { get; set; }

    public int TestId { get; set; }

    public int UserId { get; set; }

    public virtual Test Test { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
