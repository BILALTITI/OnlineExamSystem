using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 using OnlineExamSystem.Models;
using OnlineExamSystem.Models.interfaces;
 using OnlineExamSystem.Models.ViewModel;

namespace OnlineExamSystem.Controllers
{
    [Route("Exams")]
    public class ExamsController : Controller
    {
        private readonly IExam _ExamRepoistory;
        private readonly IQuestion _QuestionRepoistory;


        public ExamsController(IExam ExamRepoistory,IQuestion  QuestionRepoistory)
        {
            _ExamRepoistory = ExamRepoistory;
            _QuestionRepoistory = QuestionRepoistory;
        }
        [HttpPost("Submit")]

        public async Task<IActionResult> Submit(ExamAttemptViewModel model)
        {
            var exam = await _ExamRepoistory.GetExamWithQuestionsAsync(model.ExamId);

            if (exam == null)
            {
                return NotFound("Exam not found");
            }
             var parsedAnswers = new Dictionary<int, int>();
            foreach (var answer in model.Answers)
            {
                if (int.TryParse(answer.Value, out int parsedValue))
                {
                    parsedAnswers[answer.Key] = parsedValue;
                }
            }

            var result = new ExamResultViewModel
            {
                Exam = exam,
                TotalQuestions = exam.Questions.Count,
                UserAnswers = parsedAnswers,
                CorrectAnswersDict = exam.Questions.ToDictionary(q => q.QuestionId, q => q.CorrectAnswer)
            };

            // Calculate correct answers
            result.CorrectAnswers = result.CorrectAnswersDict.Count(kv =>
                parsedAnswers.TryGetValue(kv.Key, out var userAnswer) &&
                userAnswer == kv.Value
            );

            // Calculate percentage
            result.ScorePercentage = result.TotalQuestions > 0
                ? Math.Round((double)result.CorrectAnswers / result.TotalQuestions * 100, 2)
                : 0;

            // Determine pass/fail
            result.IsPassed = result.ScorePercentage >= 60;

            return View("Results", result);
        }
        [HttpGet("StartExam")]
        public async Task<IActionResult> StartExam(int id)
        {
            var exam = await _ExamRepoistory.GetExamWithQuestionsAsync(id);

            if (exam == null)
            {
                return NotFound();
            }

            var viewModel = new ExamAttemptViewModel
            {
                ExamId = exam.ExamId,
                ExamTitle = exam.Title,
                ExamDescription = exam.Description,
                Questions = exam.Questions?.ToList() ?? new List<Question>(),
                AttemptId = Guid.NewGuid()
            };

            return View(viewModel);
         }
         [HttpGet("Index")]
        [Authorize]  
        public async Task<IActionResult> Index()
        {
            var exams = await _ExamRepoistory.GetAllAsync();
            return PartialView(exams);   
        }

         
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Exam exam)
        {
            await _ExamRepoistory.AddAsync(exam);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Exam exam)
        {
            if (id != exam.ExamId)
                return BadRequest();

            await _ExamRepoistory.UpdateAsync(exam);
            return Ok();
        }
      

         [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ExamRepoistory.DeleteAsync(id);
            return Ok();
        }
    }
}
