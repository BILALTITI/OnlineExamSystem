using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.Models.Repositroy;
using OnlineExamSystem.Models;
using OnlineExamSystem.Models.interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using OnlineExamSystem.Models.ViewModel;

namespace OnlineExamSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : Controller
    {
        private readonly IQuestion _questionRepository;
        private readonly IExam _ExamsRepository;

        public QuestionController(IQuestion questionRepository, IExam examsRepository)
        {
            _questionRepository = questionRepository;
            _ExamsRepository = examsRepository;
        }

        public async Task<IActionResult> Index()
        {
         var Exams=await _ExamsRepository.GetAllAsync();
            return View(Exams);
        }

        [HttpGet("GetAllQuestions")]
        public async Task<IActionResult> GetAllQuestions()
        {
            var questions = await _questionRepository.GetAllAsync();
            return Ok(questions);
        }
        
         
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestionById(int id)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

                 [HttpPost]
        public async Task<ActionResult> CreateQuestion([FromBody] Question question)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _questionRepository.AddAsync(question);
            return CreatedAtAction(nameof(GetQuestionById), new { id = question.QuestionId }, question);
        }

         
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuestion(int id, [FromBody] Question updatedQuestion)
        {
            if (id != updatedQuestion.QuestionId)
                return BadRequest("Mismatched Question ID.");

            var existingQuestion = await _questionRepository.GetByIdAsync(id);
            if (existingQuestion == null)
                return NotFound();

            await _questionRepository.UpdateAsync(updatedQuestion);
            return NoContent();
        }

         
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestion(int id)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null)
                return NotFound();

            await _questionRepository.DeleteAsync(id);
            return NoContent();
        }

            }
}
