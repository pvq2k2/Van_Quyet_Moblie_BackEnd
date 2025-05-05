namespace Van_Quyet_Moblie_BackEnd.Handle.Request.SubCategoriesRequest
{
    public class UpdateSubCategoriesRequest
    {
        public string? Name { get; set; }
        public IFormFile? Image { get; set; }
        public int CategoriesID { get; set; }
    }
}
