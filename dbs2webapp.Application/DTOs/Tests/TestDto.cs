using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Tests
{
    public class TestDto
    {
        public string Title { get; set; } = default!;
        public int ChapterId { get; set; }
        public List<QuestionDto> Questions { get; set; } = new();
    }
}
