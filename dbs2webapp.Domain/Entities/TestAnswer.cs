using dbs2webapp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbs2webapp.Domain.Entities
{
    public class TestAnswer
    {
        [Key]
        public int Id { get; set; }

        public int TestResultId { get; set; }
        public TestResult TestResult { get; set; } = default!;

        public int QuestionId { get; set; }
        public Question Question { get; set; } = default!;

        public int SelectedOptionId { get; set; }
        public Option SelectedOption { get; set; } = default!;
    }

}
