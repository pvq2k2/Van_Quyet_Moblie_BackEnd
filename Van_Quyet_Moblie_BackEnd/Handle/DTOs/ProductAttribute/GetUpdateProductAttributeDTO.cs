namespace Van_Quyet_Moblie_BackEnd.Handle.DTOs.ProductAttribute
{
    public class GetUpdateProductAttributeDTO
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int ColorID { get; set; }
        public int SizeID { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
