using PracticeWebApi.CommonClasses.Orders;
using System.Threading.Tasks;

namespace PracticeWebApi.Services.Orders
{
    public class OrderService : IOrderService
    {
        public Task<Order> CreateOrder(Order order)
        {
            throw new System.NotImplementedException();
        }

        public Task<Order> FindOrderByUserId(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
