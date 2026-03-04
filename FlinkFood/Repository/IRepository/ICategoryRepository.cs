using FlinkFood.Data;

namespace FlinkFood.Repository.IRepository
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Category category);
        Task<Category?> FindById(int id);
        Task<IEnumerable<Category>> GetAllAsync();
    }
}
