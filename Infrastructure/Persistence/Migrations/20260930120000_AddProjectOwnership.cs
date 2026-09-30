using Infrastructure.Persistence.DbInitializer;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20260930120000_AddProjectOwnership")]
public partial class AddProjectOwnership : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "OwnerId",
            table: "Projects",
            type: "nvarchar(450)",
            maxLength: 450,
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Projects_OwnerId",
            table: "Projects",
            column: "OwnerId");

        migrationBuilder.AddForeignKey(
            name: "FK_Projects_AspNetUsers_OwnerId",
            table: "Projects",
            column: "OwnerId",
            principalTable: "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey("FK_Projects_AspNetUsers_OwnerId", "Projects");
        migrationBuilder.DropIndex("IX_Projects_OwnerId", "Projects");
        migrationBuilder.DropColumn("OwnerId", "Projects");
    }
}
