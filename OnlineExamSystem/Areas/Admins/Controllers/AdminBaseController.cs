using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.Models.Repositroy;
using OnlineExamSystem.Models;
using OnlineExamSystem.Models.interfaces;

namespace OnlineExamSystem.Areas.Admins.Controllers
{
    [Area("Admins")]
    [Authorize(Roles = "Admin")]
    public class AdminBaseController : Controller
    {
        private readonly IExam _ExamRepoistory;  
        private readonly IQuestion _QuestionRepoistory;  
                                                                       
        public AdminBaseController(IExam ExamRepoistory, IQuestion  QuestionRepoistory)
        {
            _ExamRepoistory = ExamRepoistory;
            _QuestionRepoistory = QuestionRepoistory;

        }
        // إدارة الامتحانات
        public async Task<IActionResult> Index2()
        {
            var exams = await _ExamRepoistory.GetAllAsync();
            return View(exams);
        }

        // إضافة امتحان جديد
        public IActionResult CreateExam()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateExam(Exam exam)
        {
            if (ModelState.IsValid)
            {
                await _ExamRepoistory.AddAsync(exam);
                return RedirectToAction(nameof(Index));
            }
            return View(exam);
        }

        // إدارة الأسئلة الخاصة بالامتحانات
        public async Task<IActionResult> ManageQuestions(int examId)
        {
            var questions = await _QuestionRepoistory.GetAllAsync();
            return View(questions);
        }

        // إضافة سؤال لامتحان معين
        public IActionResult CreateQuestion(int examId)
        {

                var question = new Question { ExamId = examId };
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion(Question question)
        {
            if (ModelState.IsValid)
            {
                await _QuestionRepoistory.AddAsync(question);
                return RedirectToAction(nameof(ManageQuestions), new { examId = question.ExamId });
            }
            return View(question);
        }

    }
}
