using CoreLayer.Dtos;
using CoreLayer.Entities;
using CoreLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : CustomBaseController
	{
		private readonly IService<Product, ProductDto> _productService;

		public ProductController(IService<Product, ProductDto> productService)
		{
			_productService = productService;
		}

		[HttpPost]
		public async Task<IActionResult> GetProducts()
		{
			return ActionResultInstance(await _productService.GetAllAsync());
		}

        [HttpPost]
        public async Task<IActionResult> SaveProducts(ProductDto dto)
        {
            return ActionResultInstance(await _productService.AddAsync(dto));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto dto)
        {
            return ActionResultInstance(await _productService.Update(dto,dto.Id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return ActionResultInstance(await _productService.Remove(id));
        }
    }
}
