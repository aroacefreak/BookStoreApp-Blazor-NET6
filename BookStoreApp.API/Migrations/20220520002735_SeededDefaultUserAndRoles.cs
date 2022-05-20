using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreApp.API.Migrations
{
    public partial class SeededDefaultUserAndRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "34806b21-f35a-48f6-899f-c1302ea2a220", "7b4f5706-ba18-453f-a56b-37547b9048bc", "User", "USER" },
                    { "f4de428c-c5a6-486a-9b48-2664a14e9607", "661d4f41-f87a-42a1-8088-5869d4f8c1fe", "Administration", "ADMINISTRATION" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "903d090a-e862-4b21-8dfc-4d068c262884", 0, "b8cb170e-6862-4500-af9c-def5e3da8544", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAEAACcQAAAAEDeQuxt4Ls5kak6/yZOYMU5YPyfMNxXVVlgkcfMo2dyvtb8JWPXfNyGl6kMGmUdNjw==", null, false, "0464f2c1-2095-4416-8e1f-9581d27a904e", false, "user@bookstore.com" },
                    { "d1710f5f-dae2-4095-acee-e12c9784d4d9", 0, "0ac67428-b5c1-43c0-80e5-ccc1bdbc760a", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAEAACcQAAAAEG3Yy7t6v5VYPuADIqDvc6d76eB0i9jHrTrP7IBQqes/WIXew7X8Bh/4A1Z3QfF3RA==", null, false, "7672ed81-91bc-42df-a5a7-3e239a96af1c", false, "admin@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "34806b21-f35a-48f6-899f-c1302ea2a220", "903d090a-e862-4b21-8dfc-4d068c262884" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f4de428c-c5a6-486a-9b48-2664a14e9607", "d1710f5f-dae2-4095-acee-e12c9784d4d9" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "34806b21-f35a-48f6-899f-c1302ea2a220", "903d090a-e862-4b21-8dfc-4d068c262884" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f4de428c-c5a6-486a-9b48-2664a14e9607", "d1710f5f-dae2-4095-acee-e12c9784d4d9" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "34806b21-f35a-48f6-899f-c1302ea2a220");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4de428c-c5a6-486a-9b48-2664a14e9607");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "903d090a-e862-4b21-8dfc-4d068c262884");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d1710f5f-dae2-4095-acee-e12c9784d4d9");
        }
    }
}
