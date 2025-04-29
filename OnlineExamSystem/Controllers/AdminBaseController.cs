using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.Models.Repositroy;
using OnlineExamSystem.Models;
using OnlineExamSystem.Models.interfaces;

namespace OnlineExamSystem.Controllers
{
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
         public async Task<IActionResult> Index2()
        {
            var exams = await _ExamRepoistory.GetAllAsync();
            return View(exams);
        }

        [HttpGet]
        public IActionResult CreateExam()
        {
            return View(new Exam
            {
                Questions = new List<Question>(),
                CreatedDate = DateTime.UtcNow
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateExam(Exam exam)
        {
             exam.CreatedDate = DateTime.UtcNow;

             if (exam.Questions == null || !exam.Questions.Any())
            {
                ModelState.AddModelError("", "At least one question is required");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _ExamRepoistory.AddAsync(exam);
                    return RedirectToAction("Index2", "AdminBase");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error saving exam: {ex.Message}");
                }
            }

             exam.Questions ??= new List<Question>();
            return View(exam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _ExamRepoistory.DeleteAsync(id);
                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditExam(int id)
        {
            var exam = await _ExamRepoistory.GetByIdAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            return View("CreateExam", exam);
        }

        [HttpPost]
        public async Task<IActionResult> EditExam(Exam exam)
        {
            if (ModelState.IsValid)
            {
                await _ExamRepoistory.UpdateAsync(exam);
                return RedirectToAction(nameof(Index2));
            }
            return View("CreateExam", exam);
        }

        [HttpGet]
        public async Task<IActionResult> GetExam(int id)
        {
            var exam = await _ExamRepoistory.GetExamWithQuestionsAsync(id);
            return Json(exam);
        }
 

     }
}
