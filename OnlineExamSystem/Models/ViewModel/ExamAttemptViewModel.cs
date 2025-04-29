using OnlineExamSystem.Models;

namespace OnlineExamSystem.Models.ViewModel
{
    public class ExamAttemptViewModel
    {
      public int ExamId { get; set; }
        public string ExamTitle { get; set; }
        public string ExamDescription { get; set; }
        public List<Question> Questions { get; set; }
        public TimeSpan TimeLeft { get; set; }
        public Guid AttemptId { get; set; }
        public Dictionary<int, string> Answers { get; set; } = new();
    }
}