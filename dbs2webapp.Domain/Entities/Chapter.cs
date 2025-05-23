﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbs2webapp.Domain.Entities
{
    public class Chapter
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Chapter name is required")]
        [MaxLength(100, ErrorMessage = "Max length of Name is 100")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Chapter description is required")]
        [MaxLength(1000, ErrorMessage = "Max length of Description is 1000")]
        public string? Description { get; set; }

        [Column(TypeName = "nvarchar(MAX)")] // For rich HTML content
        public string? Content { get; set; }

        public int Order { get; set; } = 0; // For sorting chapters..

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }

        public List<Test>? Tests { get; set; }
    }

}
