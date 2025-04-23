using dbs2webapp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbs2webapp.Application.DTOs
{
    public class ChapterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public string? Content { get; set; }

        public int Order { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CourseId { get; set; }
    }
}
