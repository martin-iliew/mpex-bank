using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MpexWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedCVVInCardModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("452d2017-83ab-4106-96b2-6da36c44c01c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c2d918b8-d9f5-4bcd-b6dd-fb628d905f4c"));

            migrationBuilder.AddColumn<string>(
                name: "CVV",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("9e91d4e3-76e8-445f-868a-c271999b7f06"), null, "User", "USER" },
                    { new Guid("e659f921-2c37-4d22-b8ba-a0e600cf2edc"), null, "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9e91d4e3-76e8-445f-868a-c271999b7f06"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e659f921-2c37-4d22-b8ba-a0e600cf2edc"));

            migrationBuilder.DropColumn(
                name: "CVV",
                table: "Cards");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("452d2017-83ab-4106-96b2-6da36c44c01c"), null, "Administrator", "ADMINISTRATOR" },
                    { new Guid("c2d918b8-d9f5-4bcd-b6dd-fb628d905f4c"), null, "User", "USER" }
                });
        }
    }
}
