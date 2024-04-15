using Microsoft.EntityFrameworkCore;
using Van_Quyet_Moblie_BackEnd.Entities;

namespace Van_Quyet_Moblie_BackEnd.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
        public DbSet<Account> Account { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Decentralization> Decentralization { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<ProductReview> ProductReview { get; set; }
        public DbSet<ProductAttribute> ProductAttribute { get; set; }
        public DbSet<Entities.Color> Color { get; set; }
        public DbSet<Entities.Size> Size { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Slides> Slides { get; set; }
        public DbSet<SubCategories> SubCategories { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Voucher> Voucher { get; set; }
        public DbSet<UserVoucher> UserVoucher { get; set; }

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
