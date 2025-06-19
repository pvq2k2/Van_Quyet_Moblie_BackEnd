namespace Van_Quyet_Moblie_BackEnd.Handle.Request.ProductRequest
{
    public class GetAllProductRequest
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public int CategoryID { get; set; } = 0;
        public int SubCategoryID { get; set; } = 0;
        public int Price { get; set; } = 0;
        public int Storage { get; set; } = 0;
        public int Option { get; set; } = 0;
    }
}
