using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbs2webapp.Domain.Entities
{
    public class Option
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string? Text { get; set; }
        [Required]
        public bool IsCorrect { get; set; }

        [Required]
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Question? Question { get; set; }
    }

}
