using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.Slides;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class SlidesConverter
    {
        public SlidesDTO EntitySlidesToDTO(Slides slides)
        {
            return new SlidesDTO { 
                ID = slides.ID,
                Image = slides.Image,
                Status = slides.Status,
                ProductID = slides.ProductID,
                SubTitle = slides.SubTitle,
                Title = slides.Title
            };
        }

        public GetActiveSlidesDTO EntitySlidesToGetActiveSlidesDTO(Slides slides)
        {
            return new GetActiveSlidesDTO
            {
                Image = slides.Image,
                SubTitle = slides.SubTitle,
                Title = slides.Title,
                ProductSlug = slides.ProductSlug,
            };
        }

        public List<GetActiveSlidesDTO> ListEntitySlidesToGetActiveSlidesDTO(List<Slides> listSlides)
        {
            var listSlidesDTO = new List<GetActiveSlidesDTO>();
            foreach (var item in listSlides)
            {
                listSlidesDTO.Add(EntitySlidesToGetActiveSlidesDTO(item));
            }

            return listSlidesDTO;
        }

        public List<SlidesDTO> ListEntitySlidesToDTO(List<Slides> listSlides) {
            var listSlidesDTO = new List<SlidesDTO>();
            foreach (var item in listSlides)
            {
                listSlidesDTO.Add(EntitySlidesToDTO(item));
            }

            return listSlidesDTO;
        }
    }
}
