using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.Models
{
    public class Question
    {
        
            public int QuestionId { get; set; }

            [Required]
            public string Text { get; set; }

            [Required]
            public string Option1 { get; set; }

            [Required]
            public string Option2 { get; set; }

            [Required]
            public string Option3 { get; set; }

            [Required]
            public string Option4 { get; set; }

            [Range(1, 4)]
            public int CorrectAnswer { get; set; }

            public int ExamId { get; set; }
            public Exam Exam { get; set; }
        }
 
}
