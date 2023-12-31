﻿using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.State;
using DressUpExchange.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DressUpExchange.API.Controllers
{
    [Route("api/payment")]
    [Controller]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _service;
        public PaymentController(IPaymentService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<ActionResult> CreatePayment(CreatePaymentRequest req)
        {
            var rs = await _service.PaymentAsync(req.TotalAmount * 100);
            return rs != null? Ok(new
            {
                url = rs
            }) : BadRequest();
        }
        [Authorize(Roles = RoleNames.Customer)]
        [HttpPost("order-payment")]
        public async Task<ActionResult> CreateOrderPayment([FromBody] OrderRequest req)
        {
            //var check = _service.CheckQuantityProduct(req.);
            //if (check)
            //{
                var rs = await _service.OrderPaymentAsync(req);
                return rs != null ? Ok(new
                {
                    url = rs
                }) : BadRequest();
            //}
            //return NotFound();
                
        }
    }
}
