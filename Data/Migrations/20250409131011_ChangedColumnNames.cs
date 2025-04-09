using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Projects",
                newName: "ImageName");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Clients",
                newName: "ImageName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Projects",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Clients",
                newName: "ImageUrl");
        }
    }
}
