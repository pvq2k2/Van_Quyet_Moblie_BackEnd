using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Van_Quyet_Moblie_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class V5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImage_Color_ColorID",
                table: "ProductImage");

            migrationBuilder.AlterColumn<int>(
                name: "ColorID",
                table: "ProductImage",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImage_Color_ColorID",
                table: "ProductImage",
                column: "ColorID",
                principalTable: "Color",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImage_Color_ColorID",
                table: "ProductImage");

            migrationBuilder.AlterColumn<int>(
                name: "ColorID",
                table: "ProductImage",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImage_Color_ColorID",
                table: "ProductImage",
                column: "ColorID",
                principalTable: "Color",
                principalColumn: "ID");
        }
    }
}
