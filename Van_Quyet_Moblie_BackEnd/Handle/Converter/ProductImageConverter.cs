using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class ProductImageConverter
    {
        public ProductImageDTO EntityProductImageToDTO(ProductImage productImage)
        {
            return new ProductImageDTO
            {
                ProductImageID = productImage.ID,
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
