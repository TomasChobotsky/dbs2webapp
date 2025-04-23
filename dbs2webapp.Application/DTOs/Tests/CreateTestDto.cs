using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbs2webapp.Application.DTOs.Tests
{
    public class CreateTestDto
    {
        public string Title { get; set; } = string.Empty;
        public int ChapterId { get; set; }
        public List<QuestionDto> Questions { get; set; } = new();
    }
}
