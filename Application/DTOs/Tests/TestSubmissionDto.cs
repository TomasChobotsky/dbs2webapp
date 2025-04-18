using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Tests
{
    public class TestSubmissionDto
    {
        public int TestId { get; set; }
        public List<AnswerSubmissionDto> Answers { get; set; } = new();
    }
}
