using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PointOfSale.Migrations
{
    public partial class ChangeDecimalByDouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TotalPrice",
                table: "Sales",
                type: "float(2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,2)",
                oldPrecision: 2);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Modifiers",
                type: "float(2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,2)",
                oldPrecision: 2);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Items",
                type: "float(2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,2)",
                oldPrecision: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Sales",
                type: "decimal(2,2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(2)",
                oldPrecision: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Modifiers",
                type: "decimal(2,2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(2)",
                oldPrecision: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Items",
                type: "decimal(2,2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(2)",
                oldPrecision: 2);
        }
    }
}
