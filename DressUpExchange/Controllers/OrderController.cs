using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DressUpExchange.API.Controllers
{
    [Route("/api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GeneralOrderResponse>> GetOrder(int id)
        {
            GeneralOrderResponse generalOrderResponse = await _orderService.GetOrderByCustomer(id);
            return Ok(generalOrderResponse);


        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderRequest orderRequest)
        {
            await _orderService.AddNewOrder(orderRequest);
            return Ok("Đơn hàng đã được tạo");
        }
    }
}
