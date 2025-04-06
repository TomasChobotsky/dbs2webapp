using System.ComponentModel.DataAnnotations;

namespace dbs2webapp.Entities
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        public List<Chapter>? Chapters { get; set; }
    }

}
