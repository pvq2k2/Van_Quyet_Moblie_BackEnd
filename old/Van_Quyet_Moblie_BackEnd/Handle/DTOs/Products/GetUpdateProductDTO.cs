namespace Van_Quyet_Moblie_BackEnd.Handle.DTOs.Products
{
    public class GetUpdateProductDTO
    {
        public int ID { get; set; }
        public int SubCategoriesID { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public int? Discount { get; set; }
        public int Status { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Weight { get; set; }
    }
}
