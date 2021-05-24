using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend6.Data.Migrations
{
    public partial class AddMessageAttachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessageAttachment_ForumMessages_ForumMessageId",
                table: "ForumMessageAttachment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumMessageAttachment",
                table: "ForumMessageAttachment");

            migrationBuilder.RenameTable(
                name: "ForumMessageAttachment",
                newName: "ForumMessageAttachments");

            migrationBuilder.RenameIndex(
                name: "IX_ForumMessageAttachment_ForumMessageId",
                table: "ForumMessageAttachments",
                newName: "IX_ForumMessageAttachments_ForumMessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumMessageAttachments",
                table: "ForumMessageAttachments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessageAttachments_ForumMessages_ForumMessageId",
                table: "ForumMessageAttachments",
                column: "ForumMessageId",
                principalTable: "ForumMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumMessageAttachments_ForumMessages_ForumMessageId",
                table: "ForumMessageAttachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumMessageAttachments",
                table: "ForumMessageAttachments");

            migrationBuilder.RenameTable(
                name: "ForumMessageAttachments",
                newName: "ForumMessageAttachment");

            migrationBuilder.RenameIndex(
                name: "IX_ForumMessageAttachments_ForumMessageId",
                table: "ForumMessageAttachment",
                newName: "IX_ForumMessageAttachment_ForumMessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumMessageAttachment",
                table: "ForumMessageAttachment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumMessageAttachment_ForumMessages_ForumMessageId",
                table: "ForumMessageAttachment",
                column: "ForumMessageId",
                principalTable: "ForumMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
