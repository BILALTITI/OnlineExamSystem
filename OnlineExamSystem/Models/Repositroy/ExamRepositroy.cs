using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.Models.interfaces;

namespace OnlineExamSystem.Models.Repositroy
{
    public class ExamRepositroy : IExam
    {
        private readonly ApplicationDbContext _context;
        public ExamRepositroy(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Exam>> GetAllAsync()
        {
            return await _context.Exams.ToListAsync();
        }
        [HttpGet("{id}")]

        public async Task<Exam> GetByIdAsync(int id)
        {
            return await _context.Exams
                .Include(e => e.Questions) // Include questions
                .FirstOrDefaultAsync(e => e.ExamId == id);
        }

        public async Task UpdateAsync(Exam exam)
        {
            var existingExam = await _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefaultAsync(e => e.ExamId == exam.ExamId);

            if (existingExam == null) return;

            // Update exam properties
            existingExam.Title = exam.Title;
            existingExam.Description = exam.Description;

            // Handle questions
            var incomingIds = exam.Questions.Select(q => q.QuestionId).ToList();

            // Remove questions not present in the incoming list
            foreach (var existingQuestion in existingExam.Questions.ToList())
            {
                if (!incomingIds.Contains(existingQuestion.QuestionId))
                    _context.Questions.Remove(existingQuestion);
            }

            // Update or add questions
            foreach (var question in exam.Questions)
            {
                var existingQuestion = existingExam.Questions
                    .FirstOrDefault(q => q.QuestionId == question.QuestionId);

                if (existingQuestion != null)
                {
                    // Update existing question
                    existingQuestion.Text = question.Text;
                    existingQuestion.Option1 = question.Option1;
                    existingQuestion.Option2 = question.Option2;
                    existingQuestion.Option3 = question.Option3;
                    existingQuestion.Option4 = question.Option4;
                    existingQuestion.CorrectAnswer = question.CorrectAnswer;
                }
                else
                {
                    // Add new question
                    existingExam.Questions.Add(question);
                }
            }

            await _context.SaveChangesAsync();
        }
        //public async Task<Exam> GetByIdAsync(int id)
        //{
        //    return await _context.Exams.FindAsync(id);
        //}

        public async Task AddAsync(Exam exam)
        {
            await _context.Exams.AddAsync(exam);
            await _context.SaveChangesAsync();
        }

        //public async Task UpdateAsync(Exam exam)
        //{
        //    _context.Exams.Update(exam);
        //    await _context.SaveChangesAsync();
        //}
        public async Task<Exam> GetExamWithQuestionsAsync(int id)
        {
            return await _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefaultAsync(e => e.ExamId == id);   
        }
        public async Task DeleteAsync(int id)
        {
            var exam = await GetByIdAsync(id);
            if (exam != null)
            {
                _context.Exams.Remove(exam);
                await _context.SaveChangesAsync();
            }
        }
    } 
}
