namespace OnlineExamSystem.Models.ViewModel
{
    public class ExamTakingVM
    {
        public int ExamId { get; set; }
        public string ExamTitle { get; set; }
        public List<QuestionVM> Questions { get; set; }
    }
}
