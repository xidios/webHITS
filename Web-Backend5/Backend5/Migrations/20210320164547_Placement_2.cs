using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend5.Migrations
{
    public partial class Placement_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Placements",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Placements_PatientId",
                table: "Placements",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Placements_Patients_PatientId",
                table: "Placements",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Placements_Patients_PatientId",
                table: "Placements");

            migrationBuilder.DropIndex(
                name: "IX_Placements_PatientId",
                table: "Placements");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Placements");
        }
    }
}
