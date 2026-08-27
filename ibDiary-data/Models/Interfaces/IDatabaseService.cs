using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_data.Models.Interfaces
{
    public interface IDatabaseService<T> where T : class
    {
        public Task<List<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(int Id);
        public Task<bool> UpdateAsync(T item);
        public Task<int> AddAsync(T item);
        public Task<bool> DeleteAsync(T item);
    }
}
