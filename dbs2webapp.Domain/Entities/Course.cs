using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbs2webapp.Domain.Entities
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Course name is required")]
        [MaxLength(100, ErrorMessage = "Max length of Name is 100")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Chapter description is required")]
        [MaxLength(1000, ErrorMessage = "Max length of Name is 1000")]
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
