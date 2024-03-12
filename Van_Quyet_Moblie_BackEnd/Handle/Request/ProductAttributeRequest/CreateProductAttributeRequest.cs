namespace Van_Quyet_Moblie_BackEnd.Handle.Request.ProductAttributeRequest
{
    public class CreateProductAttributeRequest
    {
        public int ColorID { get; set; }
        public int SizeID { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
