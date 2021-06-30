using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class migration5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultStoreId",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Customers_DefaultStoreId",
            //    table: "Customers",
            //    column: "DefaultStoreId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Customers_Stores_DefaultStoreId",
            //    table: "Customers",
            //    column: "DefaultStoreId",
            //    principalTable: "Stores",
            //    principalColumn: "StoreId",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Stores_DefaultStoreId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_DefaultStoreId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DefaultStoreId",
                table: "Customers");
        }
    }
}
