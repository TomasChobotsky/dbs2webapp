using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbs2webapp.Domain.Entities
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string? Content { get; set; }

        [Required]
        public int TestId { get; set; }
        [ForeignKey("TestId")]
        public Test? Test { get; set; }

        public List<Option>? Options { get; set; }

        public ICollection<TestAnswer> Answers { get; set; } = new List<TestAnswer>();
    }

}
