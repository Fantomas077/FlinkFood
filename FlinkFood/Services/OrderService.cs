using FlinkFood.Data;
using FlinkFood.Repository.IRepository;

namespace FlinkFood.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _order;
        public OrderService(IOrderRepository order)
        {
            _order = order;
        }

        public async Task <IEnumerable<OrderHeader>>GettAllAsync()
        {
           return  await _order.GettAll();
        }
    }
}
