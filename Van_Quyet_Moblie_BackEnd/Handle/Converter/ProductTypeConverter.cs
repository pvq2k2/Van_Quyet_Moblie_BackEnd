using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class ProductTypeConverter
    {
        public ProductTypeDTO EntityProductTypeToDTO(ProductType productType)
        {
            return new ProductTypeDTO
            {
                ProductTypeID = productType.ID,
                NameProductType = productType.NameProductType,
                ImageTypeProduct = productType.ImageTypeProduct
            };
        }

        public List<ProductTypeDTO> ListEntityProductTypeToDTO(List<ProductType> listProductType)
        {
            var productTypeDTO = new List<ProductTypeDTO>();

            foreach (var item in listProductType)
            {
                productTypeDTO.Add(EntityProductTypeToDTO(item));
            }

            return productTypeDTO;
        }
    }
}
