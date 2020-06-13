using PracticeWebApi.CommonClasses.Orders;
using System.Threading.Tasks;

namespace PracticeWebApi.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(Order order);
        Task<Order> FindOrderByUserId(string id);
    }
}
