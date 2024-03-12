using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductAttributeRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductImageRequest;

namespace Van_Quyet_Moblie_BackEnd.Handle.Request.ProductRequest
{
    public class CreateProductRequest
    {
        public string? Name { get; set; }
        public double Price { get; set; }
        public int SubCategoriesID { get; set; }
        public IFormFile? Image { get; set; }
        public string? Description { get; set; }
        public int? Discount { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Weight { get; set; }
        public List<CreateProductImageRequest>? ListProductImage { get; set; }
        public List<CreateProductAttributeRequest>? ListProductAttribute { get; set;}
    }
}
