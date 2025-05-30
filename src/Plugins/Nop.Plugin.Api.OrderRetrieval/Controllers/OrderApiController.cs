using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Orders;
using Nop.Services.Orders;
using Nop.Services.Customers;
using Nop.Web.Framework.Controllers;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Nop.Plugin.Api.OrderRetrieval.Controllers
{
    [Route("api/order")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderApiController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public OrderApiController(IOrderService orderService, ICustomerService customerService)
        {
            _orderService = orderService;
            _customerService = customerService;
        }

        [HttpGet("by-email")]
        public async Task<IActionResult> GetOrdersByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { message = "Email is required." });

            var customer = await _customerService.GetCustomerByEmailAsync(email);
            if (customer == null)
                return NotFound(new { message = "Customer not found." });

            var orders = await _orderService.SearchOrdersAsync(customerId: customer.Id);

            var result = orders.Select(o => new
            {
                OrderId = o.Id,
                TotalAmount = o.OrderTotal,
                OrderDate = o.CreatedOnUtc
            });

            return Ok(result);
        }
    }
}