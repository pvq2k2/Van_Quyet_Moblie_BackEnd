namespace Van_Quyet_Moblie_BackEnd.Handle.DTOs.User
{
    public class GetUpdateUserDTO
    {
        public int ID { get; set; }
        public string? FullName { get; set; }
        public string? NumberPhone { get; set; }
        public int Gender { get; set; }
        public string? Email { get; set; }
        public int Status { set; get; }
        public object? Address { get; set; } = null;
        //public string? DetailAddress { get; set; }
        //public int ProvinceID { get; set; }
        //public int DistrictID { get; set; }
        //public int WardID { get; set; }
    }
}
