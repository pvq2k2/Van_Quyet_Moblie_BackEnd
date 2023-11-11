namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class Decentralization : BaseEntity
    {
        public string? AuthorityName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public List<Account>? ListAccount { get; set; }
    }
}
