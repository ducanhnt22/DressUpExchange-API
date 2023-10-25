using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace DressUpExchange.API.Controllers
{
    [Route("api/send-sms")]
    [ApiController]
    public class SendSMSController : ControllerBase
    {
        private readonly ISendSSMSService _service;
        string accountSid = "AC17deb66380793cce7888c4189d07e562";
        string AuthToken = "4016fd2ffdaa4de4fabbc76b97616ce2";

        public SendSMSController(ISendSSMSService service)
        {
            _service = service;
        }


        [HttpPost("SendOTP")]
        public async Task<ActionResult> SendSMS(string? telephoneNumber)
        {
         await   _service.ForgetPassword(telephoneNumber);
          
            return StatusCode(200, new
            {
                message = "Check Your Phone Number"
            });
        }


        [HttpPost("ConfirmOTP")]
        public async Task<ActionResult> ConfirmPassword(int otp)
        {
        bool check = await    _service.ConfirmPassword(otp);

            if (check)
            {
                return Ok(new
                {
                    message = "Correct OTP"
                });
            }

            return BadRequest(new
            {
                message = "Wrong OTP"
            });

        }

        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword(string phoneNumber, [FromBody] ForgetPasswordRequestt req)
        {
        bool check =  await  _service.ChangePassword(phoneNumber, req.passwordChange);
            if (check)
            {
                return Ok(new
                {
                    message = "Change Password Successfully!"
                });
            }
            return BadRequest(new
            {
                message = "Change Password Fail!"
            });
        }
    }
}
