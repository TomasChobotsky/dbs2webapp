using dbs2webapp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbs2webapp.Application.DTOs.Tests
{
    public class TestDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Test title is required")]
        [MaxLength(100, ErrorMessage = "Max length of Title is 100")]
        public string Title { get; set; } = default!;
        [Required(ErrorMessage = "Field is required")]
        public int ChapterId { get; set; }
        public List<QuestionDto> Questions { get; set; } = new();
    }
}
