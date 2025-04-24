using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbs2webapp.Application.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course name is required")]
        [MaxLength(100, ErrorMessage = "Max length of Name is 100")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Chapter description is required")]
        [MaxLength(1000, ErrorMessage = "Max length of Name is 1000")]
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? TeacherId { get; set; }
    }
}
