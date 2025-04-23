using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbs2webapp.Application.DTOs.Tests.Result
{
    public class TestResultDetailsDto
    {
        public int Id { get; set; }
        public string TestTitle { get; set; } = string.Empty;
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public DateTime CompletedDate { get; set; }
        public List<QuestionResultDto> Questions { get; set; } = new();
    }
}
