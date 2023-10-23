using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.DTO.State;
using DressUpExchange.Service.Services;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace DressUpExchange.API.Controllers
{
    [Route("api/voucher")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVourcherService _voucherService;
        private readonly IClaimsService _claimsService;
        public VoucherController(IVourcherService vourcherService, IClaimsService claimsService)
        {
            _voucherService = vourcherService;
            _claimsService = claimsService;
        }

        [Authorize(Roles = RoleNames.Customer)]
        [HttpPost("CreateVoucher")]
        public async Task<ActionResult> CreateVoucher(int ProductID, [FromBody] VoucherRequest voucherRequest)
        {
            var userId = _claimsService.GetCurrentUserId;
            if (userId == null)
            {
                return StatusCode(401);
            } else
            {
                voucherRequest.UserId = userId;
            }
            await _voucherService.CreateNewVoucher(ProductID, voucherRequest);
            return Ok(new
            {
                msg = "Create Voucher Successfully"
            });
        }
        [Authorize(Roles = RoleNames.Customer)]
        [HttpPut]
        public async Task<ActionResult> UpdradeVoucher(int ProductID, [FromBody] UpdateVoucherRequest updateVoucherRequest)
        {
            await _voucherService.UpdateVoucher(ProductID, updateVoucherRequest);
            return Ok(new
            {
                msg = "Update voucher sucessfully"
            });
        }

        [Authorize(Roles = RoleNames.Customer)]
        [HttpDelete]
        public async Task<ActionResult> DeleteVoucher(int VoucherID)
        {
            await _voucherService.DeleteVoucher(VoucherID);
            return Ok(new
            {
                msg = "Delete Voucher Sucessfully"
            });
        }

        //[Authorize(Roles = RoleNames.Customer)]
        //[HttpPost("SaveVoucher")]
        //public async Task<ActionResult> SaveVoucher(int VoucherID, int CustomerID)
        //{
        //    await _voucherService.SaveVoucherByID(VoucherID, CustomerID);
        //    return Ok("Vocuher đã được thêm vào danh sách voucher của bạn");
        //}

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
