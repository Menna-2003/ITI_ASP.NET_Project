using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI_ASP.NET_Project.Migrations
{
    /// <inheritdoc />
    public partial class assQuantitytoCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "cart",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "cart");
        }
    }
}
