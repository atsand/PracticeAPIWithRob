using PracticeWebApi.CommonClasses.Products;
using PracticeWebApi.CommonClasses.Users;
using System.Collections.Generic;

namespace PracticeWebApi.CommonClasses.Orders
{
    public class Order : BaseResource
    {
        public string Status { get; set; }
        public User User { get; set; }
        public IList<Product> Products { get; set; }
    }
}
