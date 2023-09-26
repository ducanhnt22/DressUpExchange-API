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
            return Ok(result);
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
            var rs = await _customerService.LoginAsync(model);
            return Ok(rs);
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterRequest model)
        {
            var rs = await _customerService.RegisterAsync(model);
            return Ok(rs);
        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserResponse>> EditProfile(int id, [FromBody] UserRequest request)
        {
            var rs = await _customerService.UpdateAsync(id, request);
            return Ok(rs);
        }
        [HttpPost("RefreshToken")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RefreshTokenResponse>> RefreshToken(string refreshToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Refresh token is required", "Please provide a valid refresh token.");
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
    }
}
