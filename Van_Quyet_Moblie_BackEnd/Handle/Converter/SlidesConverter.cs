using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class SlidesConverter
    {
        public SlidesDTO EntitySlidesToDTO(Slides slides)
        {
            return new SlidesDTO { 
                Image = slides.Image,
                Status = slides.Status,
            };
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
