using FlinkFood.Data;
using FlinkFood.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FlinkFood.Repository
{
    public class ProductRepository:IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task AddAsync(Product obj)
        {
            await _db.Product.AddAsync(obj);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product obj)
        {
            _db.Product.Remove(obj);
            await _db.SaveChangesAsync();
        }

        public async Task<Product?> FindById(int id)
        {
            return await _db.Product.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Product.Include(r=>r.Category).ToListAsync();
        }

        public async Task UpdateAsync(Product obj)
        {
            _db.Product.Update(obj);
            await _db.SaveChangesAsync();
        }
        public async Task<Product?> FindByNameAsync(string name)
        {
            return await _db.Product.FirstOrDefaultAsync(r => r.Name == name);
        }
        public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        {
            return await _db.Product
                .AnyAsync(r => r.Name == name && (excludeId == null || r.Id != excludeId));
        }

    }
}

