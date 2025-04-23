using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbs2webapp.Domain.Entities
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public List<Chapter>? Chapters { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string? TeacherId { get; set; }

        [ForeignKey("TeacherId")]
        public IdentityUser? Teacher { get; set; }

        public List<UserCourse>? UserCourses { get; set; }
    }
}
