using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyWebServerProject.Migrations
{
    /// <inheritdoc />
    public partial class IpAdressAddedOnUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27665cdf-10a9-4478-8893-fc72b6d00528");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4c55a442-e7b9-4103-9086-d8a34db21ced");

            migrationBuilder.AddColumn<string>(
                name: "IpAdress",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2069535d-6d90-4c60-9a4e-af22388ed0dc", null, "Admin", "ADMIN" },
                    { "e9047223-b85b-41fd-b6a5-cb6fabdfc471", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2069535d-6d90-4c60-9a4e-af22388ed0dc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e9047223-b85b-41fd-b6a5-cb6fabdfc471");

            migrationBuilder.DropColumn(
                name: "IpAdress",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "27665cdf-10a9-4478-8893-fc72b6d00528", null, "User", "USER" },
                    { "4c55a442-e7b9-4103-9086-d8a34db21ced", null, "Admin", "ADMIN" }
                });
        }
    }
}
