using Van_Quyet_Moblie_BackEnd.Entities;

namespace Van_Quyet_Moblie_BackEnd.Handle.DTOs
{
    public class AccountDTO
    {
        public int AccountID { get; set; }
        public string? UserName { get; set; }
        public DecentralizationDTO? DecentralizationDTO { set; get; }
        public UserDTO? UserDTO { get; set; }
    }
}
