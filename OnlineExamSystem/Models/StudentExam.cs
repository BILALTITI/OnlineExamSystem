// Models/UserExamResult.cs
using OnlineExamSystem.Models;
using OnlineExamSystem.Models.interfaces;

public class StudentExam  
{
    public int Id { get; set; }

    public string StudentId { get; set; }
    public ApplicationUser Student { get; set; }

    public int ExamId { get; set; }
    public Exam Exam { get; set; }

    public int Score { get; set; }
}