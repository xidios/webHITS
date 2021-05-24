using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend6.Data.Migrations
{
    public partial class AddTopicsCreator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "ForumTopics",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopics_CreatorId",
                table: "ForumTopics",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumTopics_AspNetUsers_CreatorId",
                table: "ForumTopics",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumTopics_AspNetUsers_CreatorId",
                table: "ForumTopics");

            migrationBuilder.DropIndex(
                name: "IX_ForumTopics_CreatorId",
                table: "ForumTopics");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "ForumTopics");
        }
    }
}
