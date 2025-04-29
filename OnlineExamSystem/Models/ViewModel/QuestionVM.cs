namespace OnlineExamSystem.Models.ViewModel
{
    public class QuestionVM
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public List<string> Options { get; set; }
    }
}
