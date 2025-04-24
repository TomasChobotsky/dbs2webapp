using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbs2webapp.Domain.Entities
{
    public class Test
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Test title is required")]
        [MaxLength(100, ErrorMessage = "Max length of Title is 100")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Field is required")]
        public int ChapterId { get; set; }
        [ForeignKey("ChapterId")]
        public Chapter? Chapter { get; set; }

        public List<Question>? Questions { get; set; }
    }

}
