using FlinkFood.Data;

namespace FlinkFood.Repository.IRepository
{
    public interface IProductRepository
    {
        Task AddAsync(Product obj);
        Task UpdateAsync(Product obj);
        Task DeleteAsync(Product obj);
        Task<Product?> FindById(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> FindByNameAsync(string name);
        Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
    }
}
