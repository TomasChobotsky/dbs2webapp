using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Tests
{
    public class QuestionDto
    {
        public string Content { get; set; } = string.Empty;
        public int CorrectOptionIndex { get; set; }
        public List<OptionDto> Options { get; set; } = new();
    }
}
