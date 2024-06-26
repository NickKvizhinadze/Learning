using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiverBooks.OrderProcessing.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCreatedDateToOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BilligAddress_Street2",
                schema: "orderprocessing",
                table: "Orders",
                newName: "BillingAddress_Street2");

            migrationBuilder.RenameColumn(
                name: "BilligAddress_Street1",
                schema: "orderprocessing",
                table: "Orders",
                newName: "BillingAddress_Street1");

            migrationBuilder.RenameColumn(
                name: "BilligAddress_State",
                schema: "orderprocessing",
                table: "Orders",
                newName: "BillingAddress_State");

            migrationBuilder.RenameColumn(
                name: "BilligAddress_PostalCode",
                schema: "orderprocessing",
                table: "Orders",
                newName: "BillingAddress_PostalCode");

            migrationBuilder.RenameColumn(
                name: "BilligAddress_Country",
                schema: "orderprocessing",
                table: "Orders",
                newName: "BillingAddress_Country");

            migrationBuilder.RenameColumn(
                name: "BilligAddress_City",
                schema: "orderprocessing",
                table: "Orders",
                newName: "BillingAddress_City");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                schema: "orderprocessing",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                schema: "orderprocessing",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "BillingAddress_Street2",
                schema: "orderprocessing",
                table: "Orders",
                newName: "BilligAddress_Street2");

            migrationBuilder.RenameColumn(
                name: "BillingAddress_Street1",
                schema: "orderprocessing",
                table: "Orders",
                newName: "BilligAddress_Street1");

            migrationBuilder.RenameColumn(
                name: "BillingAddress_State",
                schema: "orderprocessing",
                table: "Orders",
                newName: "BilligAddress_State");

            migrationBuilder.RenameColumn(
                name: "BillingAddress_PostalCode",
                schema: "orderprocessing",
                table: "Orders",
                newName: "BilligAddress_PostalCode");

            migrationBuilder.RenameColumn(
                name: "BillingAddress_Country",
                schema: "orderprocessing",
                table: "Orders",
                newName: "BilligAddress_Country");

            migrationBuilder.RenameColumn(
                name: "BillingAddress_City",
                schema: "orderprocessing",
                table: "Orders",
                newName: "BilligAddress_City");
        }
    }
}
