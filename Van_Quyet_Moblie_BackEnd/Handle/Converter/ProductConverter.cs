using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.Products;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class ProductConverter
    {
        public ProductDTO EntityProductToDTO(Product product)
        {
            return new ProductDTO
            {
                Name = product.Name,
                Price = product.Price,
                Image = product.Image,
                Discount = product.Discount,
                Status = product.Status,
                NumberOfViews = product.NumberOfViews,
            };
        }

        public List<ProductDTO> ListEntityProductToDTO(List<Product> listProduct) {
            var listProductDTO = new List<ProductDTO>();
            foreach (var item in listProduct)
            {
                listProductDTO.Add(EntityProductToDTO(item));
            }

            return listProductDTO;
        }
    }
}
