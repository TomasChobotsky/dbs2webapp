using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbs2webapp.Application.DTOs
{
    public class CourseSummaryDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public int ChapterCount { get; set; }
        public int EnrollmentCount { get; set; }
    }
}
