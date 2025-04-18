using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Tests
{
    public class TestResultDto
    {
        public int Id { get; set; }
        public string TestTitle { get; set; } = string.Empty;
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public DateTime CompletedDate { get; set; }
    }
}
