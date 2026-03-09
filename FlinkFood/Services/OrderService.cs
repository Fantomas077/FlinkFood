using FlinkFood.Data;
using FlinkFood.Repository.IRepository;
using FlinkFood.Services.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace FlinkFood.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _order;
        private readonly AuthenticationStateProvider _auth;

        public OrderService(IOrderRepository order, AuthenticationStateProvider auth)
        {
            _order = order;
            _auth = auth;
        }

        public async Task<IEnumerable<OrderHeader>> GettAllAsync()
        {
            return await _order.GettAll();
        }

        public async Task AddOrderAsync(OrderHeader obj, List<ShoppingCart> cartItems)
        {
            var authState = await _auth.GetAuthenticationStateAsync();
            var user = authState.User;

            // Récupérer l'ID utilisateur (string)
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                throw new Exception("UserId claim not found");

            obj.UserId = userIdClaim;

            // Email
            obj.Email = user.FindFirst(ClaimTypes.Email)?.Value;

            // Statut + date
            obj.Status = Status.Pending;
            obj.Orderdate = DateTime.Now;

            // Génération des OrderDetails
            foreach (var item in cartItems)
            {
                obj.OrderDetails.Add(new OrderDetails
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Count = item.Count,
                    Price = item.Product.Price
                });
            }

            // Calcul du total
            obj.OrderTotal = obj.OrderDetails.Sum(d => d.Price * d.Count);

            // Sauvegarde
            await _order.AddOrderAsync(obj);

            
        }

        public async Task<OrderHeader?> getById(int id)
        {
            var obj = await _order.getById(id);
            if (obj == null)
                throw new NotFoundException("id not found");

            return obj;
        }

        public async Task<OrderHeader?> GetByUser(string user)
        {
            var authState = await _auth.GetAuthenticationStateAsync();
            var user1 = authState.User;

            var userIdClaim = user1.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim != null)
                user = userIdClaim;

            return await _order.GetByUser(user);
        }
    }
}
