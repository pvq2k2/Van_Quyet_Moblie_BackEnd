﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Van_Quyet_Moblie_BackEnd.Helpers.DBContext;

#nullable disable

namespace Van_Quyet_Moblie_BackEnd.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231006031441_V4")]
    partial class V4
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Account", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("DecentralizationID")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResetPasswordToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ResetPasswordTokenExpiry")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VerificationToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("VerifiedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("DecentralizationID");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Cart", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Cart");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.CartItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("CartID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("CartID");

                    b.HasIndex("ProductID");

                    b.ToTable("CartItem");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Decentralization", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("AuthorityName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.ToTable("Decentralization");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Order", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<double>("ActualPrice")
                        .HasColumnType("float");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Fee")
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<string>("OrderCodeGHN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderStatusID")
                        .HasColumnType("int");

                    b.Property<double>("OriginalPrice")
                        .HasColumnType("float");

                    b.Property<int>("PaymentID")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("OrderStatusID");

                    b.HasIndex("PaymentID");

                    b.HasIndex("UserID");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.OrderDetail", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<double>("PriceTotal")
                        .HasColumnType("float");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("OrderID");

                    b.HasIndex("ProductID");

                    b.ToTable("OrderDetail");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.OrderStatus", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("StatusName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("OrderStatus");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Payment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentMethod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("AvatarImageProduct")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Discount")
                        .HasColumnType("int");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<string>("NameProduct")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfViews")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("ProductTypeID")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ProductTypeID");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.ProductImage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageProduct")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.ToTable("ProductImage");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.ProductReview", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("ContentRated")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContentSeen")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("PointEvaluation")
                        .HasColumnType("int");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.HasIndex("UserID");

                    b.ToTable("ProductReview");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.ProductType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageTypeProduct")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameProductType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.ToTable("ProductType");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.RefreshToken", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("AccountID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ExpiredTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("AccountID")
                        .IsUnique();

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Slides", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.ToTable("Slides");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("AccountID")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("AccountID")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.UserVoucher", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("VoucherID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.HasIndex("VoucherID");

                    b.ToTable("UserVoucher");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Voucher", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("DiscountPercentage")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("MinimumPurchaseAmount")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Voucher");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Account", b =>
                {
                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.Decentralization", "Decentralization")
                        .WithMany("ListAccount")
                        .HasForeignKey("DecentralizationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Decentralization");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Cart", b =>
                {
                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.User", "User")
                        .WithMany("ListCart")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.CartItem", b =>
                {
                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.Cart", "Cart")
                        .WithMany("ListCartItem")
                        .HasForeignKey("CartID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.Product", "Product")
                        .WithMany("ListCartItem")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Order", b =>
                {
                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.OrderStatus", "OrderStatus")
                        .WithMany("ListOrder")
                        .HasForeignKey("OrderStatusID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.Payment", "Payment")
                        .WithMany("ListOrder")
                        .HasForeignKey("PaymentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.User", "User")
                        .WithMany("ListOrder")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrderStatus");

                    b.Navigation("Payment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.OrderDetail", b =>
                {
                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.Order", "Order")
                        .WithMany("ListOrderDetail")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.Product", "Product")
                        .WithMany("ListOrderDetail")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Product", b =>
                {
                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.ProductType", "ProductType")
                        .WithMany("ListProduct")
                        .HasForeignKey("ProductTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductType");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.ProductImage", b =>
                {
                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.Product", "Product")
                        .WithMany("ListProductImage")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.ProductReview", b =>
                {
                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.Product", "Product")
                        .WithMany("ListProductReview")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.User", "User")
                        .WithMany("ListProductReview")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.RefreshToken", b =>
                {
                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.Account", "Account")
                        .WithOne("RefreshToken")
                        .HasForeignKey("Van_Quyet_Moblie_BackEnd.Entities.RefreshToken", "AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Slides", b =>
                {
                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.User", b =>
                {
                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.Account", "Account")
                        .WithOne("User")
                        .HasForeignKey("Van_Quyet_Moblie_BackEnd.Entities.User", "AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.UserVoucher", b =>
                {
                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.Voucher", "Voucher")
                        .WithMany()
                        .HasForeignKey("VoucherID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Voucher");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Voucher", b =>
                {
                    b.HasOne("Van_Quyet_Moblie_BackEnd.Entities.User", null)
                        .WithMany("Voucher")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Account", b =>
                {
                    b.Navigation("RefreshToken");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Cart", b =>
                {
                    b.Navigation("ListCartItem");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Decentralization", b =>
                {
                    b.Navigation("ListAccount");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Order", b =>
                {
                    b.Navigation("ListOrderDetail");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.OrderStatus", b =>
                {
                    b.Navigation("ListOrder");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Payment", b =>
                {
                    b.Navigation("ListOrder");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.Product", b =>
                {
                    b.Navigation("ListCartItem");

                    b.Navigation("ListOrderDetail");

                    b.Navigation("ListProductImage");

                    b.Navigation("ListProductReview");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.ProductType", b =>
                {
                    b.Navigation("ListProduct");
                });

            modelBuilder.Entity("Van_Quyet_Moblie_BackEnd.Entities.User", b =>
                {
                    b.Navigation("ListCart");

                    b.Navigation("ListOrder");

                    b.Navigation("ListProductReview");

                    b.Navigation("Voucher");
                });
#pragma warning restore 612, 618
        }
    }
}
