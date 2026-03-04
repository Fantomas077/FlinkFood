using FlinkFood.Data;
using FlinkFood.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FlinkFood.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task AddAsync(Category category)
        {
            await _db.Category.AddAsync(category);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category category)
        {
            _db.Category.Remove(category);
            await _db.SaveChangesAsync();
        }

        public async Task<Category?> FindById(int id)
        {
            return await _db.Category.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _db.Category.ToListAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _db.Category.Update(category);
            await _db.SaveChangesAsync();
        }
        public async Task<Category?> FindByNameAsync(string name)
        {
            return await _db.Category.FirstOrDefaultAsync(r => r.Name == name);
        }
        public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        {
            return await _db.Category
                .AnyAsync(r => r.Name == name && (excludeId == null || r.Id != excludeId));
        }

    }
}
