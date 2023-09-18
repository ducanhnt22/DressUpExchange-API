using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<List<UserResponse>>> GetCustomer([FromQuery] PagingRequest pagingRequest, [FromQuery] CustomerRequest customerRequest)
        {
            var result = await _customerService.GetCustomers(customerRequest, pagingRequest);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
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
        public async Task<ActionResult<UserResponse>> EditProfile(int id, [FromBody] UserRequest request)
        {
            var rs = await _customerService.UpdateAsync(id, request);
            return Ok(rs);
        }
    }
}
