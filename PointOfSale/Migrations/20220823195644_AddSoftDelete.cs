using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PointOfSale.Migrations
{
    public partial class AddSoftDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Modifiers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Items");
        }
    }
}
