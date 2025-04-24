using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbs2webapp.Application.DTOs.Tests.Result
{
    public class OptionResultDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Option name is required")]
        [MaxLength(200, ErrorMessage = "Max length of Text is 200")]
        public string Text { get; set; } = string.Empty;
        [Required(ErrorMessage = "IsCorrect is required")]
        public bool IsCorrect { get; set; }
        public bool WasChosen { get; set; }
    }
}
