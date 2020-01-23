using Microsoft.EntityFrameworkCore.Migrations;

namespace Taste.DataAccess.Migrations
{
    public partial class MenuItemToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItem_FoodTypes_FoodTypeID",
                table: "MenuItem");

            migrationBuilder.RenameColumn(
                name: "FoodTypeID",
                table: "MenuItem",
                newName: "FoodTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_MenuItem_FoodTypeID",
                table: "MenuItem",
                newName: "IX_MenuItem_FoodTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItem_FoodTypes_FoodTypeId",
                table: "MenuItem",
                column: "FoodTypeId",
                principalTable: "FoodTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItem_FoodTypes_FoodTypeId",
                table: "MenuItem");

            migrationBuilder.RenameColumn(
                name: "FoodTypeId",
                table: "MenuItem",
                newName: "FoodTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_MenuItem_FoodTypeId",
                table: "MenuItem",
                newName: "IX_MenuItem_FoodTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItem_FoodTypes_FoodTypeID",
                table: "MenuItem",
                column: "FoodTypeID",
                principalTable: "FoodTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
