using PracticeWebApi.CommonClasses.Orders;
using PracticeWebApi.CommonClasses.Products;
using PracticeWebApi.CommonClasses.Users;
using PracticeWebApi.Data;
using PracticeWebApi.Data.Orders;
using PracticeWebApi.Data.Products;
using PracticeWebApi.Data.Users;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace PracticeWebApi.Services.Orders
{
    public class OrderMapper : IMapper<Order, OrderDataEntity>
    {
        public Order MapToBase(OrderDataEntity dataEntity)
        {
            throw new NotImplementedException();
        }

        public OrderDataEntity MapToDataEntity(Order baseType)
        {
            throw new NotImplementedException();
        }
    }
}
