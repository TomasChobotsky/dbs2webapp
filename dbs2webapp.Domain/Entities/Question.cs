using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbs2webapp.Domain.Entities
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Qustion content is required")]
        [MaxLength(200, ErrorMessage = "Max length of Content is 200")]
        public string? Content { get; set; }

        [Required(ErrorMessage = "Field is required")]
        public int TestId { get; set; }
        [ForeignKey("TestId")]
        public Test? Test { get; set; }

        public List<Option>? Options { get; set; }

        public ICollection<TestAnswer> Answers { get; set; } = new List<TestAnswer>();
    }

}
