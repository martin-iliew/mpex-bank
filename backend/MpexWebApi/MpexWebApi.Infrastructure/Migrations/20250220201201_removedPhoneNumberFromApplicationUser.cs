using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MpexWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removedPhoneNumberFromApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("464e6857-9531-4e2f-94ac-45449604952e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("baaa68aa-711d-45a0-ae30-3ad29e054fba"));

            migrationBuilder.DropColumn(
                name: "PhoneNmber",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("0a59d332-35c7-4f37-9e97-ab831993c6a8"), null, "Administrator", "ADMINISTRATOR" },
                    { new Guid("cdca0b3b-e62c-4699-a306-a4845da7370d"), null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0a59d332-35c7-4f37-9e97-ab831993c6a8"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cdca0b3b-e62c-4699-a306-a4845da7370d"));

            migrationBuilder.AddColumn<string>(
                name: "PhoneNmber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("464e6857-9531-4e2f-94ac-45449604952e"), null, "User", "USER" },
                    { new Guid("baaa68aa-711d-45a0-ae30-3ad29e054fba"), null, "Administrator", "ADMINISTRATOR" }
                });
        }
    }
}
