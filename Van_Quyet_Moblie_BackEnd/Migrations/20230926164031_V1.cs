using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Van_Quyet_Moblie_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Fee",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrderCodeGHN",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Fee",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderCodeGHN",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Order");
        }
    }
}
