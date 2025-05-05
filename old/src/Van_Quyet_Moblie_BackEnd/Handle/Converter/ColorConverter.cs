using Van_Quyet_Moblie_BackEnd.Handle.DTOs;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class ColorConverter
    {
        public ColorDTO EntityColorToDTO(Entities.Color color)
        {
            return new ColorDTO
            {
                ID = color.ID,
                Name = color.Name,
                Value = color.Value,
            };
        }

        public List<ColorDTO> ListEntityColorToDTO(List<Entities.Color> listColor)
        {
            var listColorDTO = new List<ColorDTO>();
            foreach (var color in listColor)
            {
                listColorDTO.Add(EntityColorToDTO(color));
            }

            return listColorDTO;
        }
    }
}
