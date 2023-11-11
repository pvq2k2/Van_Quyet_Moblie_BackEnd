﻿namespace Van_Quyet_Moblie_BackEnd.Entities
{
    public class Slides : BaseEntity
    {
        public string? Image { get; set; }
        public int Status { get; set; }
        public int ProductID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public Product? Product { get; set; }
    }
}
