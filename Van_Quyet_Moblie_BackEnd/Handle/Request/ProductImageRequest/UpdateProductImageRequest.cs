﻿namespace Van_Quyet_Moblie_BackEnd.Handle.Request.ProductImageRequest
{
    public class UpdateProductImageRequest
    {
        public string? Title { get; set; }
        public IFormFile? Image { get; set; }
        public int ProductID { get; set; }
        public int Status { get; set; }
    }
}