using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Handle.DTOs;

namespace Van_Quyet_Moblie_BackEnd.Handle.Converter
{
    public class UserConverter
    {
        public UserDTO EntityUserToDTO(User user)
        {
            return new UserDTO { 
                UserID = user.ID,
                FullName = user.FullName,
                Phone = user.Phone,
                Avatar = user.Avatar,
                Email = user.Email,
                Address = user.Address,
            };
        }
    }
}
