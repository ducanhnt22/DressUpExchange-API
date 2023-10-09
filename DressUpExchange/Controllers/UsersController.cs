using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.DTO.State;
using DressUpExchange.Service.Exceptions;
using DressUpExchange.Service.Services;
using Google.Apis.Auth.OAuth2.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTQ.Sdk.Core.Filters;
using System.Data;
using System.Net;

namespace DressUpExchange.API.Controllers
{
    
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public UsersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<ActionResult<List<UserResponse>>> GetCustomer([FromQuery] PagingRequest pagingRequest, [FromQuery] CustomerRequest customerRequest)
        {
            var result = await _customerService.GetCustomers(customerRequest, pagingRequest);
            return result != null ? Ok(result) : NotFound();
        }
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserResponse>> GetCustomer(int id)
        {
            var result = await _customerService.GetCustomerById(id);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Login([FromBody] LoginRequest model)
        {
            var checkPhoneFormat = _customerService.CheckPhone(model.Phone);
            if (checkPhoneFormat == false)
            {
                return StatusCode(400, new
                {
                    message = "Số điện thoại không đúng với định dạng!"
                });
            }
            var user = await _customerService.GetCustomerByPhone(model.Phone);
            if (user == null)
            {
                return StatusCode(400, new
                {
                    message = "Số điện thoại hiện chưa được đăng ký!"
                });
            }
            var checkPassword = _customerService.CheckPassword(user, model.Password);
            if (checkPassword == false)
            {
                return BadRequest(new
                {
                    message = "Mật khẩu không hợp lệ!"
                });
            }
            
            var rs = await _customerService.LoginAsync(model);
            return rs != null ? Ok(rs) : NotFound();
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterRequest model)
        {
            var checkPhoneFormat = _customerService.CheckPhone(model.PhoneNumber);
            if (checkPhoneFormat == false)
            {
                return StatusCode(400, new
                {
                    message = "Số điện thoại không đúng với định dạng!"
                });
            }
            var checkIsUniquePhone = _customerService.IsUniqueUser(model.PhoneNumber);
            if (checkIsUniquePhone == false)
            {
                return StatusCode(400, new
                {
                    message = "Số điện thoại đã đăng ký!"
                });
            }
            var rs = await _customerService.RegisterAsync(model);
            return Ok(rs);
        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserResponse>> EditProfile(int id, [FromBody] UserRequest request)
        {
            var rs = await _customerService.UpdateAsync(id, request);
            return Ok(new
            {
                message = "Thông tin tài khoản đã được cập nhật!"
            });
        }
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<RefreshTokenResponse>> RefreshToken(string refreshToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    return StatusCode(500, new { Message = "Refresh token is required" });
                }

                var refreshTokenResponse = await _customerService.RefreshTokenAsync(refreshToken);

                return Ok(refreshTokenResponse);
            }
            catch (CrudException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }
        [HttpPost("ChangePassword")]
        [Authorize(Roles = RoleNames.Customer)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest model)
        {
            try
            {
                var user = await _customerService.GetCustomerByPhone(model.PhoneNumber);
                if (user == null)
                {
                    return StatusCode(400, new
                    {
                        message = "Số điện thoại hiện chưa được đăng ký!"
                    });
                }

                var isPasswordValid = _customerService.CheckPassword(user, model.Password);
                if (!isPasswordValid)
                {
                    return BadRequest(new
                    {
                        message = "Mật khẩu hiện tại không đúng!"
                    });
                }

                user.Password = model.NewPassword;
                await _customerService.UpdateAsync(user.UserId, new UserRequest { Password = model.NewPassword });

                return Ok(new
                {
                    message = "Mật khẩu đã được thay đổi thành công!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi server khi thay đổi mật khẩu: " + ex.Message
                });
            }
        }

    }
}
