using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.ProductAttribute;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class ProductAttributeConverter
    {
        public GetUpdateProductAttributeDTO EntityProductAttributeToGetUpdateProductAttributeDTO(ProductAttribute productAttribute)
        {
            return new GetUpdateProductAttributeDTO
            {
                ID = productAttribute.ID,
                ProductID = productAttribute.ProductID,
                ColorID = productAttribute.ColorID,
                SizeID = productAttribute.SizeID,
                Quantity = productAttribute.Quantity,
                Price = productAttribute.Price,
            };
        }
        public ProductAttributeDTO EntityProductAttributeToDTO(ProductAttribute productAttribute)
        {
            return new ProductAttributeDTO
            {
                ID = productAttribute.ID,
                ColorName = productAttribute.Color?.Name,
                ColorValue = productAttribute.Color?.Value,
                SizeName = productAttribute.Size?.Name,
                Quantity = productAttribute.Quantity,
                Price = productAttribute.Price,
            };
        }

        public List<ProductAttributeDTO> ListEntityProductAttributeToDTO(List<ProductAttribute> listProductAttribute)
        {
            var listProductAttributeDTO = new List<ProductAttributeDTO>();
            foreach (var item in listProductAttribute)
            {
                listProductAttributeDTO.Add(EntityProductAttributeToDTO(item));
            }

            return listProductAttributeDTO;
        }
    }
}
