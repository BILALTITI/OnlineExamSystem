// Interfaces/IUserExamResult.cs
using OnlineExamSystem.Models;

public interface IStudentExam
{
    int UserExamResultId { get; set; }
    string UserId { get; set; }
    ApplicationUser User { get; set; }
    int ExamId { get; set; }
    Exam Exam { get; set; }
    int Score { get; set; }
    int CorrectAnswers { get; set; }
    int TotalQuestions { get; set; }
    bool PassStatus { get; set; }
    DateTime AttemptDate { get; set; }
}