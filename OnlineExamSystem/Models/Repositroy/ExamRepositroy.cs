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

        public async Task<Exam> GetByIdAsync(int id)
        {
            return await _context.Exams.FindAsync(id);
        }

        public async Task AddAsync(Exam exam)
        {
            await _context.Exams.AddAsync(exam);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Exam exam)
        {
            _context.Exams.Update(exam);
            await _context.SaveChangesAsync();
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
