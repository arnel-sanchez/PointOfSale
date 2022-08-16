using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PointOfSale.Migrations
{
    public partial class UpdateSalesModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Sales_SaleId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Modifiers_Sales_SaleId",
                table: "Modifiers");

            migrationBuilder.DropIndex(
                name: "IX_Items_SaleId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "SaleId",
                table: "Modifiers",
                newName: "ModifiersByItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Modifiers_SaleId",
                table: "Modifiers",
                newName: "IX_Modifiers_ModifiersByItemId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ModifiersByItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ItemId = table.Column<string>(type: "nvarchar(36)", nullable: false),
                    SaleId = table.Column<string>(type: "nvarchar(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModifiersByItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModifiersByItem_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModifiersByItem_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModifiersByItem_ItemId",
                table: "ModifiersByItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ModifiersByItem_SaleId",
                table: "ModifiersByItem",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modifiers_ModifiersByItem_ModifiersByItemId",
                table: "Modifiers",
                column: "ModifiersByItemId",
                principalTable: "ModifiersByItem",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modifiers_ModifiersByItem_ModifiersByItemId",
                table: "Modifiers");

            migrationBuilder.DropTable(
                name: "ModifiersByItem");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "ModifiersByItemId",
                table: "Modifiers",
                newName: "SaleId");

            migrationBuilder.RenameIndex(
                name: "IX_Modifiers_ModifiersByItemId",
                table: "Modifiers",
                newName: "IX_Modifiers_SaleId");

            migrationBuilder.AddColumn<string>(
                name: "SaleId",
                table: "Items",
                type: "nvarchar(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_SaleId",
                table: "Items",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Sales_SaleId",
                table: "Items",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Modifiers_Sales_SaleId",
                table: "Modifiers",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id");
        }
    }
}
