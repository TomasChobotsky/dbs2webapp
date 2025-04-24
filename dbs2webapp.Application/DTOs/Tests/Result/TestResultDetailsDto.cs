using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbs2webapp.Application.DTOs.Tests.Result
{
    public class TestResultDetailsDto
    {
        public int Id { get; set; }
        public string TestTitle { get; set; } = string.Empty;
        [Required(ErrorMessage = "Field is required")]
        public int Score { get; set; }
        [Required(ErrorMessage = "Field is required")]
        public int TotalQuestions { get; set; }
        [Required(ErrorMessage = "Field is required")]
        public DateTime CompletedDate { get; set; }
        public List<QuestionResultDto> Questions { get; set; } = new();
    }
}
