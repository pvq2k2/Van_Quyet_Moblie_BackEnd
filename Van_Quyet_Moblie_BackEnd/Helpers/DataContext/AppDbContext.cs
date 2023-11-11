using Microsoft.EntityFrameworkCore;
using Van_Quyet_Moblie_BackEnd.Entities;
using Van_Quyet_Moblie_BackEnd.Helpers.DataContext;

namespace Van_Quyet_Moblie_BackEnd.Helpers.DBContext
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Settings.MyConnectString());
        }
        public DbSet<Account> Account { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<Decentralization> Decentralization { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<ProductReview> ProductReview { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Voucher> Voucher { get; set; }
        public DbSet<UserVoucher> UserVoucher { get; set; }
        public DbSet<Slides> Slides { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasOne(account => account.User)
                .WithOne(user => user.Account)
                .HasForeignKey<User>(user => user.AccountID);

            modelBuilder.Entity<Account>()
                .HasOne(account => account.RefreshToken)
                .WithOne(refreshToken => refreshToken.Account)
                .HasForeignKey<RefreshToken>(refreshToken => refreshToken.AccountID);
        }
    }
}
