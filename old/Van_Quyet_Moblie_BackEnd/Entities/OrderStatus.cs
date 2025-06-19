namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class OrderStatus : BaseEntity
    {
        public string? StatusName { get; set; }

        public List<Order>? ListOrder { get; set; }
    }
}
