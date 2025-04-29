 using OnlineExamSystem.Models;

public class ExamResultViewModel
{
    public Exam Exam { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public double ScorePercentage { get; set; }
    public bool IsPassed { get; set; }

     public Dictionary<int, int> UserAnswers { get; set; }
    public Dictionary<int, int> CorrectAnswersDict { get; set; }
}