using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.DTO.State;
using DressUpExchange.Service.Services;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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

        [Authorize(Roles = RoleNames.Admin)]
        [HttpGet]
        public async Task<ActionResult<GeneralOrderResponse>> GetAllOrder([FromQuery] PagingRequest pagingRequest)
        {
            GeneralOrderResponse generalOrderResponse = await _orderService.GetOrder(pagingRequest);
            return Ok(generalOrderResponse);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GeneralOrderResponse>> GetOrder(int id, [FromQuery] OrderPagingRequest pagingRequest)
        {
            GeneralOrderResponse generalOrderResponse = await _orderService.GetOrderByCustomer(id,pagingRequest);
            return Ok(generalOrderResponse);
        }
        
        [HttpGet("orderId/{id:int}")]
        public async Task<ActionResult<GeneralOrderResponse>> GetOrderByOrderId(int id, [FromQuery] OrderPagingRequest pagingRequest)
        {
            GeneralOrderResponse generalOrderResponse = await _orderService.GetOrderByOrderId(id,pagingRequest);
            return Ok(generalOrderResponse);
        }
        [Authorize(Roles = RoleNames.Customer)]
        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderRequest orderRequest)
        {
            await _orderService.AddNewOrder(orderRequest);
            return Ok(new
            {
                Message = "Create Order Sucessfully"
            });
        }
    }
}
