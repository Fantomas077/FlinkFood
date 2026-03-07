using FlinkFood.Data;

namespace FlinkFood.Repository.IRepository
{
    public interface IShoppingCartRepository
    {
        Task<List<ShoppingCart>> GetCartAsync(string userId);
        Task<ShoppingCart?> GetItemAsync(string userId, int productId);
        Task AddAsync(ShoppingCart cart);
        Task UpdateAsync(ShoppingCart cart);
        Task RemoveAsync(ShoppingCart cart);
        Task SaveAsync();

    }
}
