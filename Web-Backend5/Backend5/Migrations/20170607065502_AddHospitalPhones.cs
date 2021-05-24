using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend5.Migrations
{
    public partial class AddHospitalPhones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HospitalPhones",
                columns: table => new
                {
                    HospitalId = table.Column<int>(nullable: false),
                    PhoneId = table.Column<int>(nullable: false),
                    Number = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospitalPhones", x => new { x.HospitalId, x.PhoneId });
                    table.ForeignKey(
                        name: "FK_HospitalPhones_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HospitalPhones");
        }
    }
}
