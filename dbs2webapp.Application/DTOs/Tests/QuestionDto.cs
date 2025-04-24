using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbs2webapp.Application.DTOs.Tests
{
    public class QuestionDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Qustion content is required")]
        [MaxLength(200, ErrorMessage = "Max length of Content is 200")]
        public string Content { get; set; } = string.Empty;
        public int CorrectOptionIndex { get; set; }
        public List<OptionDto> Options { get; set; } = new();
    }
}
