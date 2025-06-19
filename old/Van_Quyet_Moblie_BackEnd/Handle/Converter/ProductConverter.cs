using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.Products;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class ProductConverter
    {
        public GetUpdateProductDTO EntityProductToGetUpdateProductDTO(Product product)
        {
            return new GetUpdateProductDTO
            {
                ID = product.ID,
                SubCategoriesID = product.SubCategoriesID,
                Name = product.Name,
                Price = product.Price,
                Image = product.Image,
                Description = product.Description,
                Discount = product.Discount,
                Status = product.Status,
                Height = product.Height,
                Width = product.Width,
                Length = product.Length,
                Weight = product.Weight,
            };
        }
        public ProductDTO EntityProductToDTO(Product product)
        {
            return new ProductDTO
            {
                ID = product.ID,
                Name = product.Name,
                Price = product.Price,
                Image = product.Image,
                Discount = product.Discount,
                Status = product.Status,
                Slug = product.Slug,
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
