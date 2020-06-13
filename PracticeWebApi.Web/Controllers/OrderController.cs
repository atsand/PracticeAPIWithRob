using Microsoft.AspNetCore.Mvc;
using PracticeWebApi.CommonClasses.Orders;
using PracticeWebApi.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PracticeWebApi.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("/orders")]
        public async Task<IActionResult> CreateOrder([FromBody]Order order)
        {
            try
            {
                var addedOrder = await _orderService.CreateOrder(order);
                
                return Ok(addedOrder);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("/orders/{userId}")]
        public async Task<IActionResult> FindOrderByUserId([FromRoute] string userId)
        {
            try
            {
                var order = await _orderService.FindOrderByUserId(userId);

                return Ok(order);
            }
            catch (Exception exception)
            {
                //not best way to handle exception
                //will always return 404
                return BadRequest(exception.Message);
            }
        }
    }
}
