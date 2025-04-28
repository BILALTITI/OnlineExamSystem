using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.Models.Repositroy;
using OnlineExamSystem.Models;
using OnlineExamSystem.Models.interfaces;

namespace OnlineExamSystem.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestion _questionRepository;

        public QuestionController(IQuestion questionRepository)
        {
            _questionRepository = questionRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetAllQuestions()
        {
            var questions = await _questionRepository.GetAllAsync();
            return Ok(questions);
        }

        // Get Question by Id
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

        // Create New Question
        [HttpPost]
        public async Task<ActionResult> CreateQuestion([FromBody] Question question)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _questionRepository.AddAsync(question);
            return CreatedAtAction(nameof(GetQuestionById), new { id = question.QuestionId }, question);
        }

        // Update Question
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

        // Delete Question
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestion(int id)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null)
                return NotFound();

            await _questionRepository.DeleteAsync(id);
            return NoContent();
        }

        // ✅ Batch Insert (Insert Multiple Questions at Once)
        [HttpPost("batch")]
        public async Task<ActionResult> BatchInsertQuestions([FromBody] List<Question> questions)
        {
            if (questions == null || questions.Count == 0)
                return BadRequest("No questions provided.");

            foreach (var question in questions)
            {
                await _questionRepository.AddAsync(question);
            }

            return Ok("Questions inserted successfully!");
        }
    }
}
