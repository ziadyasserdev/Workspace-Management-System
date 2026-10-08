using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workspace_Management_System.Infrastructure.Migrations
{
    public partial class CreateSessionServiceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessionService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    SessionId = table.Column<int>(type: "int", nullable: false),

                    ServiceId = table.Column<int>(type: "int", nullable: false),

                    UnitPrice = table.Column<decimal>(
                        type: "decimal(18,2)",
                        precision: 18,
                        scale: 2,
                        nullable: false),

                    Quantity = table.Column<decimal>(
                        type: "decimal(18,3)",
                        precision: 18,
                        scale: 3,
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_SessionService",
                        x => x.Id);

                    table.ForeignKey(
                        name: "FK_SessionService_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_SessionService_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionService_SessionId_ServiceId",
                table: "SessionService",
                columns: new[] { "SessionId", "ServiceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionService_ServiceId",
                table: "SessionService",
                column: "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionService");
        }
    }
}