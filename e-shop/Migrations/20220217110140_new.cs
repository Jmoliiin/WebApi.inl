using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_shop.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderRows_Products_OroductsId",
                table: "OrderRows");

            migrationBuilder.DropIndex(
                name: "IX_OrderRows_OroductsId",
                table: "OrderRows");

            migrationBuilder.DropColumn(
                name: "OroductsId",
                table: "OrderRows");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRows_ProductsId",
                table: "OrderRows",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderRows_Products_ProductsId",
                table: "OrderRows",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderRows_Products_ProductsId",
                table: "OrderRows");

            migrationBuilder.DropIndex(
                name: "IX_OrderRows_ProductsId",
                table: "OrderRows");

            migrationBuilder.AddColumn<int>(
                name: "OroductsId",
                table: "OrderRows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderRows_OroductsId",
                table: "OrderRows",
                column: "OroductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderRows_Products_OroductsId",
                table: "OrderRows",
                column: "OroductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
