using FlinkFood.Data;
using System.Globalization;

namespace FlinkFood.Repository.IRepository
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(OrderHeader obj);
        Task <List<OrderHeader>> GettAll();
        Task UpdateOderAsync(OrderHeader obj);
        Task<OrderHeader?> GetByUser(string user);
        Task<OrderHeader?> getById(int id);
    }
}
