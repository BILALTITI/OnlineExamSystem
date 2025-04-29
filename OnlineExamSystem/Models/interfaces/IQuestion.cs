﻿using OnlineExamSystem.Models.ViewModel;

namespace OnlineExamSystem.Models.interfaces
{
    public interface IQuestion
    {
 
        Task<IEnumerable<Question>> GetAllAsync();
        Task<Question> GetByIdAsync(int id);
        Task AddAsync(Question question);
        Task UpdateAsync(Question question);
        Task DeleteAsync(int id);

    }
}
