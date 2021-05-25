using Microsoft.EntityFrameworkCore.Migrations;

namespace SentinelApi.Migrations
{
    public partial class AddUserRelatedFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RollNo",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SchoolCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RollNo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SchoolCode",
                table: "AspNetUsers");
        }
    }
}
