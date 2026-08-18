using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOrderApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveAndUserIdToRestaurant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Restaurants",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Restaurants",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Restaurants",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Restaurants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourierId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId1",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "UserId" },
                values: new object[] { "", null });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "UserId" },
                values: new object[] { "", null });

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_UserId",
                table: "Restaurants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CourierId",
                table: "Orders",
                column: "CourierId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_CourierId",
                table: "Orders",
                column: "CourierId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Users_UserId",
                table: "Restaurants",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId1",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_CourierId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Users_UserId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_UserId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CourierId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RestaurantId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "CourierId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RestaurantId1",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Restaurants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Restaurants",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);
        }
    }
}
