using System.Collections.Generic;

namespace OnlineExamSystem.Models.interfaces
{
    public interface IExam
    {
          Task<Exam> GetExamWithQuestionsAsync(int examId);

        Task<IEnumerable<Exam>> GetAllAsync();
        Task<Exam> GetByIdAsync(int id);
        Task AddAsync(Exam exam);
        Task UpdateAsync(Exam exam);
        Task DeleteAsync(int id);

    }
}
