using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.Services;
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
    }
}
