using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Models;

public class AssignmentUser
{
    public int Id { get; set; }

    public int AssignmentId { get; set; }

    public int UserId { get; set; }

    [ForeignKey("AssignmentId")]
    public virtual Assignment Assignment { get; set; } = null!;

    [ForeignKey("UserId")]
    public virtual AppUser User { get; set; } = null!;
}