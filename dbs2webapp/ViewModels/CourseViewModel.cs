using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "Category")]
        public int CourseCategoryId { get; set; }
    }
}
