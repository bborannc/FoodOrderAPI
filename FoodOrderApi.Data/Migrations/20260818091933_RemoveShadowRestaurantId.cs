using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOrderApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveShadowRestaurantId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RestaurantId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RestaurantId1",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "DeliveryStreet",
                table: "Orders",
                newName: "DeliveryAddress_Street");

            migrationBuilder.RenameColumn(
                name: "DeliveryNeighborhood",
                table: "Orders",
                newName: "DeliveryAddress_Neighborhood");

            migrationBuilder.RenameColumn(
                name: "DeliveryDoorNumber",
                table: "Orders",
                newName: "DeliveryAddress_DoorNumber");

            migrationBuilder.RenameColumn(
                name: "DeliveryDistrict",
                table: "Orders",
                newName: "DeliveryAddress_District");

            migrationBuilder.RenameColumn(
                name: "DeliveryCity",
                table: "Orders",
                newName: "DeliveryAddress_City");

            migrationBuilder.RenameColumn(
                name: "DeliveryBuildingNumber",
                table: "Orders",
                newName: "DeliveryAddress_BuildingNumber");

            migrationBuilder.RenameColumn(
                name: "DeliveryAddressDirections",
                table: "Orders",
                newName: "DeliveryAddress_AddressDirections");

            migrationBuilder.AlterColumn<string>(
                name: "CancellationReason",
                table: "Orders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryAddress_Street",
                table: "Orders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeliveryAddress_Street",
                table: "Orders",
                newName: "DeliveryStreet");

            migrationBuilder.RenameColumn(
                name: "DeliveryAddress_Neighborhood",
                table: "Orders",
                newName: "DeliveryNeighborhood");

            migrationBuilder.RenameColumn(
                name: "DeliveryAddress_DoorNumber",
                table: "Orders",
                newName: "DeliveryDoorNumber");

            migrationBuilder.RenameColumn(
                name: "DeliveryAddress_District",
                table: "Orders",
                newName: "DeliveryDistrict");

            migrationBuilder.RenameColumn(
                name: "DeliveryAddress_City",
                table: "Orders",
                newName: "DeliveryCity");

            migrationBuilder.RenameColumn(
                name: "DeliveryAddress_BuildingNumber",
                table: "Orders",
                newName: "DeliveryBuildingNumber");

            migrationBuilder.RenameColumn(
                name: "DeliveryAddress_AddressDirections",
                table: "Orders",
                newName: "DeliveryAddressDirections");

            migrationBuilder.AlterColumn<string>(
                name: "CancellationReason",
                table: "Orders",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryStreet",
                table: "Orders",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId1",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RestaurantId1",
                table: "Orders",
                column: "RestaurantId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId1",
                table: "Orders",
                column: "RestaurantId1",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }
    }
}
