﻿using Microsoft.AspNetCore.Mvc;
using Van_Quyet_Moblie_BackEnd.Helpers;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductRequest;
using Van_Quyet_Moblie_BackEnd.Services.Interface;

namespace Van_Quyet_Moblie_BackEnd.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _iProductService;

        public ProductController(IProductService iProductService)
        {
            _iProductService = iProductService;
        }

        [HttpPost("get-all-product")]
        public async Task<IActionResult> GetAllProduct(Pagination pagination)
        {
            return Ok(await _iProductService.GetAllProduct(pagination));
        }

        [HttpPost("get-all-product-where-comments-exist")]
        public async Task<IActionResult> GetAllProductsWhereCommentsExist(Pagination pagination)
        {
            return Ok(await _iProductService.GetAllProductsWhereCommentsExist(pagination));
        }

        [HttpPost("get-all-product-by-category")]
        public async Task<IActionResult> GetAllProductByCategory(GetAllProductRequest request)
        {
            return Ok(await _iProductService.GetAllProductByCategory(request));
        }

        [HttpGet("get-product-by-id/{productID}")]
        public async Task<IActionResult> GetProductByID([FromRoute] int productID)
        {
            return Ok(await _iProductService.GetProductByID(productID));
        }

        [HttpGet("get-update-product-by-id/{productID}")]
        public async Task<IActionResult> GetUpdateProductByID([FromRoute] int productID)
        {
            return Ok(await _iProductService.GetUpdateProductByID(productID));
        }

        [HttpGet("get-product-by-id-and-update-view/{productID}")]
        public async Task<IActionResult> GetProductByIDAndUpdateView([FromRoute] int productID)
        {
            return Ok(await _iProductService.GetProductByIDAndUpdateView(productID));
        }

        [HttpGet("get-product-by-slug/{slug}")]
        public async Task<IActionResult> GetProductBySlug([FromRoute] string slug)
        {
            return Ok(await _iProductService.GetProductBySlug(slug));
        }

        [HttpGet("get-related-products/{productID}")]
        public async Task<IActionResult> GetRelatedProducts([FromRoute] int productID)
        {
            return Ok(await _iProductService.GetRelatedProducts(productID));
        }

        [HttpGet("get-featured-product")]
        public async Task<IActionResult> GetFeaturedProduct()
        {
            return Ok(await _iProductService.GetFeaturedProduct());
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest request)
        {
            return Ok(await _iProductService.CreateProduct(request));
        }

        [HttpPut("update-product/{productID}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int productID, [FromForm] UpdateProductRequest request)
        {
            return Ok(await _iProductService.UpdateProduct(productID, request));
        }

        [HttpDelete("remove-product/{productID}")]
        public async Task<IActionResult> RemoveProduct([FromRoute] int productID)
        {
            return Ok(await _iProductService.RemoveProduct(productID));
        }
    }
}
