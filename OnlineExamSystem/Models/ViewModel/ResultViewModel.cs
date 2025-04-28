namespace OnlineExamSystem.Models.ViewModel
{
    public class ResultViewModel
    {
        
            public double Score { get; set; }
            public int TotalQuestions { get; set; }
            public int CorrectAnswers { get; set; }
            public int IncorrectAnswers { get; set; }
            public string PassFailStatus { get; set; }
       

    }
}
