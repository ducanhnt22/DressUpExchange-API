using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.DTO.State;
using DressUpExchange.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DressUpExchange.API.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;   
        private readonly IClaimsService _claimsService;
        public ProductsController(IProductService productService, IClaimsService claimsService)
        {
            _productService = productService;
            _claimsService = claimsService;
        }

        
        [HttpGet]
        public async Task<ActionResult<List<ProductResponse>>> GetProducts([FromQuery] PagingRequest pagingRequest, [FromQuery] ProductGetRequest productRequest)
        {
            var rs = await _productService.GetProducts(productRequest, pagingRequest);
            return Ok(rs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResponse>> GetProduct(int id)
        {
            var rs = await _productService.GetProductById(id);
            return Ok(rs);
        }


        [Authorize(Roles = RoleNames.Customer)]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductResponse>> UpdateProduct([FromBody] ProductRequest productRequest, int id)
        {
            var rs = await _productService.UpdateProduct(id, productRequest);
            if (rs == null) return NotFound();
            return Ok(new
            {
                message = "Thông tin sản phẩm đã được cập nhật!"
            });
        }
        [HttpPost()]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] ProductRequest model)
        {
            int id = _claimsService.GetCurrentUserId;
            if (id != null)
            {
                model.UserId = id;
            }
            else return StatusCode(401);
            var rs = await _productService.CreateProduct(model);
            return Ok(new
            {
                message = "Đăng bán sản phẩm thành công!"
            });
        }
    }
}
