using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbs2webapp.Application.DTOs.Tests
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int CorrectOptionIndex { get; set; }
        public List<OptionDto> Options { get; set; } = new();
    }
}
