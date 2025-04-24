using dbs2webapp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbs2webapp.Domain.Entities
{
    public class TestResult
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Field is required")]
        public int Score { get; set; }
        [Required(ErrorMessage = "Field is required")]
        public int TotalQuestions { get; set; }
        [Required(ErrorMessage = "Field is required")]
        public DateTime CompletedDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Field is required")]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }

        [Required(ErrorMessage = "Field is required")]
        public int TestId { get; set; }
        [ForeignKey("TestId")]
        public Test? Test { get; set; }
        public ICollection<TestAnswer> Answers { get; set; } = new List<TestAnswer>();
    }
}
