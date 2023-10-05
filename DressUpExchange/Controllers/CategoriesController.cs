using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DressUpExchange.API.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoriesController
    {
        private readonly ICategoryService _categoryService;
        private readonly IClaimsService _claimsService;
        public CategoriesController(ICategoryService categoryService, IClaimsService claimsService)
        {
            _categoryService = categoryService;
            _claimsService = claimsService;
        }
        //[HttpPost]
        //public async Task<ActionResult> CreateCategory([FromBody] CategoryRequest request)
        //{
        //    var categoryResponse = await _categoryService.CreateCategory(request);
        //    return HttpStatusCode.OK(categoryResponse);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetCategory(int id)
        //{
        //    var categoryResponse = await _categoryService.GetCategoryById(id);
        //    if (categoryResponse == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(categoryResponse);
        //}


    }
}
