using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbs2webapp.Domain.Entities
{
    public class Option
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Option name is required")]
        [MaxLength(200, ErrorMessage = "Max length of Text is 200")]
        public string? Text { get; set; }

        [Required(ErrorMessage = "IsCorrect is required")]
        public bool IsCorrect { get; set; }

        [Required(ErrorMessage = "Field is required")]
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Question? Question { get; set; }
    }

}
