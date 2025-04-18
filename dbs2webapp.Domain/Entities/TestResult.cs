using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class TestResult
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Score { get; set; }
        [Required]
        public int TotalQuestions { get; set; }
        [Required]
        public DateTime CompletedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }

        [Required]
        public int TestId { get; set; }
        [ForeignKey("TestId")]
        public Test? Test { get; set; }
    }
}
