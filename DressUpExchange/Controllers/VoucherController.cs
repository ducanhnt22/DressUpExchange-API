using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.DTO.State;
using DressUpExchange.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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

        [Authorize(Roles = RoleNames.Customer)]
        [HttpPost("CreateVoucher")]
        public async Task<ActionResult> CreateVoucher(int ProductID, [FromBody] VoucherRequest voucherRequest)
        {
            await _voucherService.CreateNewVoucher(ProductID, voucherRequest);
            return Ok("Create Voucher Successfully");
        }
        [Authorize(Roles = RoleNames.Customer)]
        [HttpPut]
        public async Task<ActionResult> UpdradeVoucher(int ProductID, [FromBody] UpdateVoucherRequest updateVoucherRequest)
        {
            await _voucherService.UpdateVoucher(ProductID, updateVoucherRequest);
            return Ok("Update voucher sucessfully");
        }

        [Authorize(Roles = RoleNames.Customer)]
        [HttpDelete]
        public async Task<ActionResult> DeleteVoucher(int VoucherID)
        {
            await _voucherService.DeleteVoucher(VoucherID);
            return Ok("Delete Voucher Sucessfully");
        }

        [Authorize(Roles = RoleNames.Customer)]
        [HttpPost("SaveVoucher")]
        public async Task<ActionResult> SaveVoucher(int VoucherID, int CustomerID)
        {
            await _voucherService.SaveVoucherByID(VoucherID, CustomerID);
            return Ok("Vocuher đã được thêm vào danh sách voucher của bạn");
        }

        [Authorize(Roles = RoleNames.Customer)]
        [HttpGet("GetVoucherByCustomerID")]
        public async Task<ActionResult<VoucherResponse>> GetVoucherByCustomer(int userID, [FromQuery] PagingRequest pagingRequest)
        {
           var voucherResponse = await _voucherService.GetVoucherByCustomerID(userID);
            return Ok(voucherResponse);

        }

        [Authorize(Roles = RoleNames.Customer)]
        [HttpGet("GetVoucherByProductID")]
        public async Task<ActionResult<VoucherResponse>> GetVoucherByProduct(int productID, [FromQuery] PagingRequest pa )
        {
            var voucherResponses = await _voucherService.GetVoucherByProductID(productID);

            return Ok(voucherResponses);
                 
        }
    }
}
