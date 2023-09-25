using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DressUpExchange.API.Controllers
{
    [Route("api/voucher")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVourcherService _voucherService;
        public VoucherController(IVourcherService vourcherService)
        {
            _voucherService = vourcherService;
        }
       
        [HttpPost("CreateVoucher")]
        public async Task<ActionResult> CreateVoucher(int ProductID, [FromBody] VoucherRequest voucherRequest)
        {
            await _voucherService.CreateNewVoucher(ProductID, voucherRequest);
            return Ok("Create Voucher Successfully");
        }

        [HttpPut]
        public async Task<ActionResult> UpdradeVoucher(int ProductID, [FromBody] UpdateVoucherRequest updateVoucherRequest)
        {
            await _voucherService.UpdateVoucher(ProductID, updateVoucherRequest);
            return Ok("Update voucher sucessfully");
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteVoucher(int VoucherID)
        {
            await _voucherService.DeleteVoucher(VoucherID);
            return Ok("Delete Voucher Sucessfully");
        }

      
        [HttpPost("SaveVoucher")]
        public async Task<ActionResult> SaveVoucher(int VoucherID, int CustomerID)
        {
            await _voucherService.SaveVoucherByID(VoucherID, CustomerID);
            return Ok("Vocuher đã được thêm vào danh sách voucher của bạn");
        }

        [HttpGet("GetVoucherByCustomerID")]
        public async Task<ActionResult<VoucherResponse>> GetVoucherByCustomer(int userID, [FromQuery] PagingRequest pagingRequest)
        {
           var voucherResponse = await _voucherService.GetVoucherByCustomerID(userID);
            return Ok(voucherResponse);

        }

        
        [HttpGet("GetVoucherByProductID")]
        public async Task<ActionResult<VoucherResponse>> GetVoucherByProduct(int productID, [FromQuery] PagingRequest pa )
        {
            var voucherResponses = await _voucherService.GetVoucherByProductID(productID);

            return Ok(voucherResponses);
                 
        }
    }
}
