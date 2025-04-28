using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace OnlineExamSystem.Models
{
    public class Exam
    {
        public int ExamId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public List<Question> Questions { get; set; } = new();
    }
}
