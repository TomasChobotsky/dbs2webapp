using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Models;

public partial class TestInstance
{
    public int Id { get; set; }

    public DateTime StartedAt { get; set; }

    public int Attempt { get; set; }

    public int TestId { get; set; }

    public int UserId { get; set; }

    [ForeignKey("TestId")]
    public virtual Test Test { get; set; } = null!;

    [ForeignKey("UserId")]
    public virtual AppUser User { get; set; } = null!;
}
