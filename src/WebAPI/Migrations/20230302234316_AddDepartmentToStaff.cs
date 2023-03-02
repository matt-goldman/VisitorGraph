using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphVisitor.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentToStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "Staff");
        }
    }
}
