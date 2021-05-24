using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend6.Data.Migrations
{
    public partial class AddForumImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForumMessageAttachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    FileName = table.Column<string>(nullable: false),
                    FilePath = table.Column<string>(nullable: false),
                    ForumMessageId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumMessageAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumMessageAttachment_ForumMessages_ForumMessageId",
                        column: x => x.ForumMessageId,
                        principalTable: "ForumMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForumMessageAttachment_ForumMessageId",
                table: "ForumMessageAttachment",
                column: "ForumMessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForumMessageAttachment");
        }
    }
}
