using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class migration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerOrders_Stores_StoreId",
                table: "CustomerOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedItems_CustomerOrders_CustomerOrderModelOrderId",
                table: "OrderedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedItems_Items_ItemsModelItemId",
                table: "OrderedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StoredItems_Items_ItemsModelItemId",
                table: "StoredItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StoredItems_Stores_StoreModelStoreId",
                table: "StoredItems");

            migrationBuilder.DropIndex(
                name: "IX_StoredItems_ItemsModelItemId",
                table: "StoredItems");

            migrationBuilder.DropIndex(
                name: "IX_StoredItems_StoreModelStoreId",
                table: "StoredItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderedItems_CustomerOrderModelOrderId",
                table: "OrderedItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderedItems_ItemsModelItemId",
                table: "OrderedItems");

            migrationBuilder.DropColumn(
                name: "ItemsModelItemId",
                table: "StoredItems");

            migrationBuilder.DropColumn(
                name: "StoreModelStoreId",
                table: "StoredItems");

            migrationBuilder.DropColumn(
                name: "CustomerOrderModelOrderId",
                table: "OrderedItems");

            migrationBuilder.DropColumn(
                name: "ItemsModelItemId",
                table: "OrderedItems");

            migrationBuilder.RenameColumn(
                name: "CustomerOrderId",
                table: "OrderedItems",
                newName: "OrderId");

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "CustomerOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoredItems_StoreId",
                table: "StoredItems",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedItems_OrderId",
                table: "OrderedItems",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerOrders_Stores_StoreId",
                table: "CustomerOrders",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedItems_CustomerOrders_OrderId",
                table: "OrderedItems",
                column: "OrderId",
                principalTable: "CustomerOrders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedItems_Items_ItemId",
                table: "OrderedItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoredItems_Items_ItemId",
                table: "StoredItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoredItems_Stores_StoreId",
                table: "StoredItems",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerOrders_Stores_StoreId",
                table: "CustomerOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedItems_CustomerOrders_OrderId",
                table: "OrderedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedItems_Items_ItemId",
                table: "OrderedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StoredItems_Items_ItemId",
                table: "StoredItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StoredItems_Stores_StoreId",
                table: "StoredItems");

            migrationBuilder.DropIndex(
                name: "IX_StoredItems_StoreId",
                table: "StoredItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderedItems_OrderId",
                table: "OrderedItems");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "OrderedItems",
                newName: "CustomerOrderId");

            migrationBuilder.AddColumn<int>(
                name: "ItemsModelItemId",
                table: "StoredItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoreModelStoreId",
                table: "StoredItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerOrderModelOrderId",
                table: "OrderedItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemsModelItemId",
                table: "OrderedItems",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "CustomerOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_StoredItems_ItemsModelItemId",
                table: "StoredItems",
                column: "ItemsModelItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StoredItems_StoreModelStoreId",
                table: "StoredItems",
                column: "StoreModelStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedItems_CustomerOrderModelOrderId",
                table: "OrderedItems",
                column: "CustomerOrderModelOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedItems_ItemsModelItemId",
                table: "OrderedItems",
                column: "ItemsModelItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerOrders_Stores_StoreId",
                table: "CustomerOrders",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedItems_CustomerOrders_CustomerOrderModelOrderId",
                table: "OrderedItems",
                column: "CustomerOrderModelOrderId",
                principalTable: "CustomerOrders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedItems_Items_ItemsModelItemId",
                table: "OrderedItems",
                column: "ItemsModelItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoredItems_Items_ItemsModelItemId",
                table: "StoredItems",
                column: "ItemsModelItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoredItems_Stores_StoreModelStoreId",
                table: "StoredItems",
                column: "StoreModelStoreId",
                principalTable: "Stores",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
