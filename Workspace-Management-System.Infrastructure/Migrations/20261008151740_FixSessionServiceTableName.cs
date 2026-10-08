using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workspace_Management_System.Infrastructure.Migrations
{
    public partial class FixSessionServiceTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionServices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}