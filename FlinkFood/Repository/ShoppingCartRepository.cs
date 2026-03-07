using FlinkFood.Data;
using FlinkFood.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly ApplicationDbContext _db;

    public ShoppingCartRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<ShoppingCart>> GetCartAsync(string userId)
    {
        return await _db.ShoppingCart
            .Where(c => c.UserId == userId)
            .Include(c => c.Product)
            .ToListAsync();
    }

    public async Task<ShoppingCart?> GetItemAsync(string userId, int productId)
    {
        return await _db.ShoppingCart
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
    }

    public async Task AddAsync(ShoppingCart cart)
    {
        await _db.ShoppingCart.AddAsync(cart);
    }

    public Task UpdateAsync(ShoppingCart cart)
    {
        _db.ShoppingCart.Update(cart);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(ShoppingCart cart)
    {
        _db.ShoppingCart.Remove(cart);
        return Task.CompletedTask;
    }

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
}
