using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.ProductImage;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class ProductImageConverter
    {
        public GetUpdateProductImageDTO EntityProductImageToGetUpdateProductImageDTO(ProductImage productImage)
        {
            return new GetUpdateProductImageDTO
            {
                ID = productImage.ID,
                Title = productImage.Title,
                Image = productImage.Image,
                ProductID = productImage.ID,
                ColorID = productImage.ColorID,
                Status = productImage.Status,
            };
        }
        public ProductImageDTO EntityProductImageToDTO(ProductImage productImage)
        {
            return new ProductImageDTO
            {
                ID = productImage.ID,
                Title = productImage.Title,
                Image = productImage.Image,
                Status = productImage.Status
            };
        }

        public List<ProductImageDTO> ListEntityProductImageToDTO(List<ProductImage> listProductImage)
        {
            var productImageDTO = new List<ProductImageDTO>();

            foreach (var item in listProductImage)
            {
                productImageDTO.Add(EntityProductImageToDTO(item));
            }

            return productImageDTO;
        }
    }
}
