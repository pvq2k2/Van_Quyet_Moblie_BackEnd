using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class DecentralizationConverter
    {
        public DecentralizationDTO EntityDecentralizationToDTO(Decentralization decentralization)
        {
            return new DecentralizationDTO { 
                ID = decentralization.ID,
                Name = decentralization.Name,
            };
        }

        public List<DecentralizationDTO> ListEntityDecentralizationToDTO(List<Decentralization> listDecentralization)
        {
            var listDecentralizationDTO = new List<DecentralizationDTO>();
            foreach (var item in listDecentralization)
            {
                listDecentralizationDTO.Add(EntityDecentralizationToDTO(item));
            }

            return listDecentralizationDTO;
        }
    }
}
