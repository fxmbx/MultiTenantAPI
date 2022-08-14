using System;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MultiTenantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        public ProductController(IProductService _productService)
        {
            productService = _productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = await productService.GetByIdAsync(id);
            if (response.Success)
            {
                return Ok(response);

            }
            return BadRequest(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateProductDto request)
        {
            var response = await productService.CreateAsync(request.Name, request.Description, request.Rate);
            if (response.Success)
            {
                return Ok(response);

            }
            return BadRequest(response);
        }
    }
}

