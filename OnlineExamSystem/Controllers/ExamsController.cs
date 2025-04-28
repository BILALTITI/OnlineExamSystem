using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.Models.interfaces;
using OnlineExamSystem.Models.Repositroy;

namespace OnlineExamSystem.Controllers
{
    [Authorize]
    public class ExamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IExam _examRepository;
        private readonly IQuestion _questionRepository;
        private readonly IUserExamResult _userExamResultRepository;

        public ExamsController(IExam examRepository, IQuestion questionRepository, IUserExamResult userExamResultRepository)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _userExamResultRepository = userExamResultRepository;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Exams.ToListAsync());
        }

        public async Task<IActionResult> Take(int id)
        {
            var exam = await _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefaultAsync(e => e.ExamId == id);

            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }
        
          

            //[HttpPost]
            //public Task  <IActionResult> SubmitExam(int examId, Dictionary<int, int> answers)
            //{
            //    var questions = _questionRepository.GetByIdAsync(examId);
            //    int correctAnswers = 0;

         
            //    // حساب الإجابات الصحيحة
            //    foreach (var question in questions)
            //    {
            //        if (answers.TryGetValue(question.Id, out int answer) && answer == question.CorrectAnswer)
            //        {
            //            correctAnswers++;
            //        }
            //    }

            //    // حساب النتيجة النهائية
            //    double totalQuestions = questions.Count;
            //    double score = (correctAnswers / totalQuestions) * 100;
            //    string passFailStatus = score >= 60 ? "ناجح" : "راسب";

            //    // حفظ النتيجة في قاعدة البيانات
            //    var userExamResult = new StudentExam
            //    {
            //        ExamId = examId,
            //         Score = score,
            //        PassFailStatus = passFailStatus
            //    };

            //    _userExamResultRepository.AddAsync(userExamResult);

            //    // إرسال النتيجة إلى الواجهة
            //    var resultViewModel = new ResultViewModel
            //    {
            //        Score = score,
            //        TotalQuestions = (int)totalQuestions,
            //        CorrectAnswers = correctAnswers,
            //        IncorrectAnswers = (int)(totalQuestions - correctAnswers),
            //        PassFailStatus = passFailStatus
            //    };

            //    return View("Result", resultViewModel);
            //}
        //}

    }
}
