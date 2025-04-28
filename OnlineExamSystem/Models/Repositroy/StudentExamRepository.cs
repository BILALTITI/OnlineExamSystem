using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.Models.interfaces;

namespace OnlineExamSystem.Models.Repositroy
{
    public class StudentExamRepository : IUserExamResult
    {
        private readonly ApplicationDbContext _context;
        public StudentExamRepository(ApplicationDbContext context)
        {
            _context = context;
        }
      

        public async Task<IEnumerable<StudentExam>> GetAllAsync()
        {
            return await _context.StudentExams
                                 .Include(se => se.Student)
                                 .Include(se => se.Exam)
                                 .ToListAsync();
        }

        public async Task<StudentExam> GetByIdAsync(int id)
        {
            return await _context.StudentExams
                                 .Include(se => se.Student)
                                 .Include(se => se.Exam)
                                 .FirstOrDefaultAsync(se => se.Id == id);
        }

        public async Task AddAsync(StudentExam userExamResult)
        {
            await _context.StudentExams.AddAsync(userExamResult);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StudentExam userExamResult)
        {
            _context.StudentExams.Update(userExamResult);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var result = await GetByIdAsync(id);
            if (result != null)
            {
                _context.StudentExams.Remove(result);
                await _context.SaveChangesAsync();
            }
        }
    }
}
