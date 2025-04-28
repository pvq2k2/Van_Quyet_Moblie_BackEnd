using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs.User;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class UserConverter
    {
        public UserDTO EntityUserToDTO(User user)
        {
            return new UserDTO
            {
                ID = user.ID,
                UserName = user.Account!.UserName,
                Status = user.Account!.Status,
                DecentralizationID = user.Account!.DecentralizationID,
                FullName = user.FullName,
                NumberPhone = user.NumberPhone,
                Avatar = user.Avatar,
                Gender = user.Gender,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
            };
        }

        public List<UserDTO> ListEntityUserToDTO(List<User> listUser)
        {
            var listUserDTO = new List<UserDTO>();
            foreach (var user in listUser)
            {
                listUserDTO.Add(EntityUserToDTO(user));
            }

            return listUserDTO;
        }
    }
}
