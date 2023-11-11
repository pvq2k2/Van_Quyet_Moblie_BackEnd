using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Van_Quyet_Moblie_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class V4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voucher_Order_OrderID",
                table: "Voucher");

            migrationBuilder.DropIndex(
                name: "IX_Voucher_OrderID",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "Voucher");

            migrationBuilder.CreateTable(
                name: "UserVoucher",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    VoucherID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVoucher", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserVoucher_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserVoucher_Voucher_VoucherID",
                        column: x => x.VoucherID,
                        principalTable: "Voucher",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserVoucher_UserID",
                table: "UserVoucher",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserVoucher_VoucherID",
                table: "UserVoucher",
                column: "VoucherID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserVoucher");

            migrationBuilder.AddColumn<int>(
                name: "OrderID",
                table: "Voucher",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_OrderID",
                table: "Voucher",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Voucher_Order_OrderID",
                table: "Voucher",
                column: "OrderID",
                principalTable: "Order",
                principalColumn: "ID");
        }
    }
}
