using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.Models;
using OnlineExamSystem.Models.interfaces;
using OnlineExamSystem.Models.Repositroy;

namespace OnlineExamSystem.Areas.Admins.Controllers
{
    public class ExamsController : Controller
    {
        private readonly IExam _ExamRepoistory;  
        public ExamsController(IExam  ExamRepoistory )
        {
            _ExamRepoistory = ExamRepoistory;
          

        }

        // GET: Admin/Exams
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var exams = await _ExamRepoistory.GetAllAsync();
            return Ok(exams);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var exam = await _ExamRepoistory.GetByIdAsync(id);
            if (exam == null)
                return NotFound();

            return Ok(exam);
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
