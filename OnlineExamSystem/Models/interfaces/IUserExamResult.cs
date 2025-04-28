namespace OnlineExamSystem.Models.interfaces
{
    public interface IUserExamResult
    {
        Task<IEnumerable<StudentExam>> GetAllAsync();
        Task<StudentExam> GetByIdAsync(int id);
        Task AddAsync(StudentExam userExamResult);
        Task UpdateAsync(StudentExam userExamResult);
        Task DeleteAsync(int id);
    }         
}
