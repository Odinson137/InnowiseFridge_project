using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnowiseFridge_project.Migrations
{
    /// <inheritdoc />
    public partial class DeleteImageFromProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
