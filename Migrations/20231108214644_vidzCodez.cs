using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalApp.Migrations
{
    public partial class vidzCodez : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bills_AppointmentId",
                table: "Bills");

            migrationBuilder.CreateTable(
                name: "CreditCards",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameOnCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CVV = table.Column<int>(type: "int", nullable: false),
                    Expiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCards", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bills_AppointmentId",
                table: "Bills",
                column: "AppointmentId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditCards");

            migrationBuilder.DropIndex(
                name: "IX_Bills_AppointmentId",
                table: "Bills");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_AppointmentId",
                table: "Bills",
                column: "AppointmentId");
        }
    }
}
