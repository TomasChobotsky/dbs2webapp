using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CourseDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? TeacherId { get; set; }
    }
}
