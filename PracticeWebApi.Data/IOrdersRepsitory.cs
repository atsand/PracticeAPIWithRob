using PracticeWebApi.CommonClasses.Orders;
using System.Threading.Tasks;

namespace PracticeWebApi.Data
{
    public interface IOrdersRepsitory
    {        Task<Order> CreateOrder(Order order);
        Task<Order> FindOrderByUserId(string id);
    }
}
