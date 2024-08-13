using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class ProductReviewConverter
    {
        public ProductReviewDTO EntityProductReviewToDTO(ProductReview productReview)
        {
            return new ProductReviewDTO
            {
                ID = productReview.ID,
                ContentRated = productReview.ContentRated,
                PointEvaluation = productReview.PointEvaluation,
                ContentSeen = productReview.ContentSeen,
                Status = productReview.Status,
            };
        }

        public List<ProductReviewDTO> ListProductReviewToDTO(List<ProductReview> listProductReview)
        {
            var listProductReviewDTO = new List<ProductReviewDTO>();
            foreach (var item in listProductReview)
            {
                listProductReviewDTO.Add(EntityProductReviewToDTO(item));
            }

            return listProductReviewDTO;
        }
    }
}
