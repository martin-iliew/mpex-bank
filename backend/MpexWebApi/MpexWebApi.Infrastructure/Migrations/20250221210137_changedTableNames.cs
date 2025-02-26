using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MpexWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccount_AspNetUsers_UserId",
                table: "BankAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_Card_BankAccount_BankAccountId",
                table: "Card");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Card",
                table: "Card");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankAccount",
                table: "BankAccount");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0a59d332-35c7-4f37-9e97-ab831993c6a8"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cdca0b3b-e62c-4699-a306-a4845da7370d"));

            migrationBuilder.RenameTable(
                name: "Card",
                newName: "Cards");

            migrationBuilder.RenameTable(
                name: "BankAccount",
                newName: "BankAccounts");

            migrationBuilder.RenameIndex(
                name: "IX_Card_BankAccountId",
                table: "Cards",
                newName: "IX_Cards_BankAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccount_UserId",
                table: "BankAccounts",
                newName: "IX_BankAccounts_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cards",
                table: "Cards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankAccounts",
                table: "BankAccounts",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("8f4f99e0-e8c1-445a-943b-28b2778d86e3"), null, "User", "USER" },
                    { new Guid("e6ba1967-1fde-436e-a85d-782c76ffbde2"), null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_AspNetUsers_UserId",
                table: "BankAccounts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_BankAccounts_BankAccountId",
                table: "Cards",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_AspNetUsers_UserId",
                table: "BankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_BankAccounts_BankAccountId",
                table: "Cards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cards",
                table: "Cards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankAccounts",
                table: "BankAccounts");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8f4f99e0-e8c1-445a-943b-28b2778d86e3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e6ba1967-1fde-436e-a85d-782c76ffbde2"));

            migrationBuilder.RenameTable(
                name: "Cards",
                newName: "Card");

            migrationBuilder.RenameTable(
                name: "BankAccounts",
                newName: "BankAccount");

            migrationBuilder.RenameIndex(
                name: "IX_Cards_BankAccountId",
                table: "Card",
                newName: "IX_Card_BankAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccounts_UserId",
                table: "BankAccount",
                newName: "IX_BankAccount_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Card",
                table: "Card",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankAccount",
                table: "BankAccount",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("0a59d332-35c7-4f37-9e97-ab831993c6a8"), null, "Administrator", "ADMINISTRATOR" },
                    { new Guid("cdca0b3b-e62c-4699-a306-a4845da7370d"), null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccount_AspNetUsers_UserId",
                table: "BankAccount",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Card_BankAccount_BankAccountId",
                table: "Card",
                column: "BankAccountId",
                principalTable: "BankAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
