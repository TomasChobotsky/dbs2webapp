using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbs2webapp.Application.DTOs
{
    public class CreateChapterDto
    {
        [Required(ErrorMessage = "Chapter name is required")]
        [MaxLength(100, ErrorMessage = "Max length of Name is 100")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Chapter description is required")]
        [MaxLength(1000, ErrorMessage = "Max length of Description is 1000")]
        public string Description { get; set; } = default!;

        public string? Content { get; set; }

        public int Order { get; set; } = 0;

        public int CourseId { get; set; }
    }
}