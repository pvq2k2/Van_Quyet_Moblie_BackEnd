using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class ProductConverter
    {
        public ProductDTO EntityProductToDTO(Product product)
        {
            return new ProductDTO
            {
                ProductID = product.ID,
                NameProduct = product.NameProduct,
                Price = product.Price,
                AvatarImageProduct = product.AvatarImageProduct,
                Title = product.Title,
                Discount = product.Discount,
                Status = product.Status,
                NumberOfViews = product.NumberOfViews,
                Height = product.Height,
                Width = product.Width,
                Length = product.Length,
                Weight = product.Weight
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
