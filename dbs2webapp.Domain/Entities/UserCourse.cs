using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbs2webapp.Domain.Entities
{
    public class UserCourse
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Field is required")]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }

        [Required(ErrorMessage = "Field is required")]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
    }

}
