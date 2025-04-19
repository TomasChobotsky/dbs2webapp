using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateChapterDto
    {
        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public string? Content { get; set; }

        public int Order { get; set; } = 0;

        public int CourseId { get; set; }
    }
}