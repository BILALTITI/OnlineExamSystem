using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.Models.interfaces;
using OnlineExamSystem.Models.ViewModel;

namespace OnlineExamSystem.Models.Repositroy
{
    public class QuestionRepositroy : IQuestion
    {
        private readonly ApplicationDbContext _context;

        public QuestionRepositroy(ApplicationDbContext context)
        {
            _context = context;
        }

        //public List<QuestionVM> GetAllQuestions()
        //{
        //    return _context.Questions
        //        .Select(q => new QuestionVM
        //        {
        //            Id = q.QuestionId,
        //            ExamId = q.ExamId,
        //            Text = q.Text,
        //            OptionA = q.Option1,
        //            OptionB = q.Option2,
        //            OptionC = q.Option3,
        //            OptionD = q.Option4
        //        })
        //        .ToList();
        //}

        public async Task<IEnumerable<Question>> GetAllAsync()
        {
            return await _context.Questions.ToListAsync();
        }

        public async Task<Question> GetByIdAsync(int id)
        {
            return await _context.Questions.FirstOrDefaultAsync(q => q.QuestionId == id);
        }

        public async Task AddAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Question question)
        {
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var question = await GetByIdAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }
        }
    }
}
