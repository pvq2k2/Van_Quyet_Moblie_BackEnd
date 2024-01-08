using Van_Quyet_Moblie_BackEnd.Handle.DTOs;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class SizeConverter
    {
        public SizeDTO EntitySizeToDTO(Entities.Size size)
        {
            return new SizeDTO
            {
                ID = size.ID,
                Name = size.Name,
                Value = size.Value,
            };
        }

        public List<SizeDTO> ListEntitySizeToDTO(List<Entities.Size> listSize)
        {
            var listSizeDTO = new List<SizeDTO>();
            foreach (var size in listSize)
            {
                listSizeDTO.Add(EntitySizeToDTO(size));
            }

            return listSizeDTO;
        }
    }
}
