using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Serilogdemo.Migrations
{
    /// <inheritdoc />
    public partial class addfieldinSellProductandPurchaseProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "SellProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "SellProducts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "SellProducts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "SellProducts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "PurchaseProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "PurchaseProducts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "PurchaseProducts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "PurchaseProducts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellProducts_CreatedBy",
                table: "SellProducts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SellProducts_UpdatedBy",
                table: "SellProducts",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseProducts_CreatedBy",
                table: "PurchaseProducts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseProducts_UpdatedBy",
                table: "PurchaseProducts",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseProducts_Users_CreatedBy",
                table: "PurchaseProducts",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseProducts_Users_UpdatedBy",
                table: "PurchaseProducts",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SellProducts_Users_CreatedBy",
                table: "SellProducts",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellProducts_Users_UpdatedBy",
                table: "SellProducts",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseProducts_Users_CreatedBy",
                table: "PurchaseProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseProducts_Users_UpdatedBy",
                table: "PurchaseProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_SellProducts_Users_CreatedBy",
                table: "SellProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_SellProducts_Users_UpdatedBy",
                table: "SellProducts");

            migrationBuilder.DropIndex(
                name: "IX_SellProducts_CreatedBy",
                table: "SellProducts");

            migrationBuilder.DropIndex(
                name: "IX_SellProducts_UpdatedBy",
                table: "SellProducts");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseProducts_CreatedBy",
                table: "PurchaseProducts");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseProducts_UpdatedBy",
                table: "PurchaseProducts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SellProducts");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "SellProducts");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SellProducts");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "SellProducts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PurchaseProducts");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "PurchaseProducts");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PurchaseProducts");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "PurchaseProducts");
        }
    }
}
