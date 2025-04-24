using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbs2webapp.Application.DTOs.Tests
{
    public class CreateTestDto
    {
        [Required(ErrorMessage = "Test title is required")]
        [MaxLength(100, ErrorMessage = "Max length of Title is 100")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Field is required")]
        public int ChapterId { get; set; }
        public List<QuestionDto> Questions { get; set; } = new();
    }
}
