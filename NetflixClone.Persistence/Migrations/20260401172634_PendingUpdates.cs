using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetflixClone.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PendingUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelAtPeriodEnd",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                table: "Subscriptions");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProfilePreferences",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Plans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Invoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "AverageRating",
                table: "Contents",
                type: "decimal(3,2)",
                precision: 3,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProfilePreferences");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Contents");

            migrationBuilder.AddColumn<bool>(
                name: "CancelAtPeriodEnd",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
