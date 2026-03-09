using FlinkFood.Data;
using FlinkFood.Repository.IRepository;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

public class ShoppingCartService
{
    private readonly IShoppingCartRepository _repo;
    private readonly AuthenticationStateProvider _auth;
    public event Action? Onchange;

    public event Func<Task>? OnChange;

    public ShoppingCartService(IShoppingCartRepository repo, AuthenticationStateProvider auth)
    {
        _repo = repo;
        _auth = auth;
    }

    private async Task<string> GetUserIdAsync()
    {
        var state = await _auth.GetAuthenticationStateAsync();
        var user = state.User;

        if (!user.Identity.IsAuthenticated)
            throw new Exception("User not authenticated");

        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value
               ?? throw new Exception("User ID not found");
    }


    public async Task<List<ShoppingCart>> GetCartAsync()
    {
        var userId = await GetUserIdAsync();
        return await _repo.GetCartAsync(userId);
    }

    public async Task AddToCartAsync(int productId)
    {
        var userId = await GetUserIdAsync();
        var existing = await _repo.GetItemAsync(userId, productId);

        if (existing == null)
        {
            await _repo.AddAsync(new ShoppingCart
            {
                UserId = userId,
                ProductId = productId,
                Count = 1
            });
        }
        else
        {
            existing.Count++;
            await _repo.UpdateAsync(existing);
        }

        await _repo.SaveAsync();

        Onchange?.Invoke();

    }

    public async Task UpdateItemAsync(ShoppingCart item)
    {
        await _repo.UpdateAsync(item);
        await _repo.SaveAsync();

        Onchange?.Invoke();
    }

    public async Task RemoveFromCartAsync(int productId)
    {
        var userId = await GetUserIdAsync();
        var item = await _repo.GetItemAsync(userId, productId);

        if (item != null)
        {
            await _repo.RemoveAsync(item);
            await _repo.SaveAsync();

            Onchange?.Invoke();
        }
    }
    public async Task ClearCartAsync()
    {
        var cart = await GetCartAsync();

        foreach (var item in cart)
        {
            await RemoveFromCartAsync(item.ProductId);
        }

        OnChange?.Invoke();
    }

}
