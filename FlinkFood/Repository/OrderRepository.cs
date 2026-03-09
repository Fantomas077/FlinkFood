using FlinkFood.Data;
using FlinkFood.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FlinkFood.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task AddOrderAsync(OrderHeader obj)
        {
            await _db.OrderHeader.AddAsync(obj);
            await _db.SaveChangesAsync();
        
        }

        public async Task<OrderHeader?> getById(int id)
        {
            return await _db.OrderHeader.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<OrderHeader?> GetByUser(int user)
        {
            return await _db.OrderHeader.FirstOrDefaultAsync(r => r.UserId == user);
        }

        public async  Task<List<OrderHeader>> GettAll()
        {
            return await _db.OrderHeader.ToListAsync();
        }

        public async Task UpdateOderAsync(OrderHeader obj)
        {
             _db.OrderHeader.Update(obj);
            await _db.SaveChangesAsync();

        }
    }
}
