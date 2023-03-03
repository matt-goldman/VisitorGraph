using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphVisitor.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class RenamedSignoutColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SingedOut",
                table: "Visits",
                newName: "SignedOut");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SignedOut",
                table: "Visits",
                newName: "SingedOut");
        }
    }
}
