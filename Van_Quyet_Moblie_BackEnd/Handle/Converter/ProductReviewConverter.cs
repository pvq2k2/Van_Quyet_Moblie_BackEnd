using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.ProductReview;

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

        public GetProductReviewToViewDTO EntityProductReviewToGetProductReviewDTO(ProductReview productReview)
        {
            return new GetProductReviewToViewDTO
            {
                ID = productReview.ID,
                ContentRated = productReview.ContentRated,
                PointEvaluation = productReview.PointEvaluation,
                CreatedAt = productReview.CreatedAt,
                FullName = productReview.User!.FullName,
                Avatar = productReview.User!.Avatar,
            };
        }

        public List<GetProductReviewToViewDTO> ListProductReviewToGetProductReviewDTO(List<ProductReview> listProductReview)
        {
            var listProductReviewDTO = new List<GetProductReviewToViewDTO>();
            foreach (var item in listProductReview)
            {
                listProductReviewDTO.Add(EntityProductReviewToGetProductReviewDTO(item));
            }

            return listProductReviewDTO;
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
