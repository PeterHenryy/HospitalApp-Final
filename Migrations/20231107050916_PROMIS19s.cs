using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalApp.Migrations
{
    public partial class PROMIS19s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PROMIS10s",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Answer1 = table.Column<int>(type: "int", nullable: false),
                    Answer2 = table.Column<int>(type: "int", nullable: false),
                    Answer3 = table.Column<int>(type: "int", nullable: false),
                    Answer4 = table.Column<int>(type: "int", nullable: false),
                    Answer5 = table.Column<int>(type: "int", nullable: false),
                    Answer6 = table.Column<int>(type: "int", nullable: false),
                    Answer7 = table.Column<int>(type: "int", nullable: false),
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROMIS10s", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PROMIS10s_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PROMIS10s_AppointmentId",
                table: "PROMIS10s",
                column: "AppointmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PROMIS10s");
        }
    }
}
