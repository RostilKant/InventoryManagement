using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class RemoveConfigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: new Guid("2b7efe32-3e84-4e46-8a8d-af5f757a0413"));

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: new Guid("2dbf4106-9194-4125-8d4d-095e10cba4f4"));

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "DeviceId",
                keyValue: new Guid("65f245ae-81b6-425d-adaf-eb3ac1a2a5b9"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("4c3c2c62-0ee8-4abb-93fa-d9bbf185d1d2"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("74257e60-490f-472f-ae81-17c9518a7ee6"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("e5efeedd-207b-422c-8637-b2a4564d0737"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "DeviceId", "Category", "EmployeeId", "Imei", "LastUpdateDate", "MacAddress", "Manufacturer", "Model", "Notes", "OfficeAddress", "PurchaseCost", "PurchaseDate", "Serial", "Status", "TenantId", "UpdateCost", "UserId", "Warranty", "WarrantyExpires" },
                values: new object[,]
                {
                    { new Guid("2b7efe32-3e84-4e46-8a8d-af5f757a0413"), "Laptops", null, "q39450ifjsdgsjdgjs12342hd", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AF-55-AF-35-CD-DF", "Asus", "Vivobook 14", "", "Kaliko Str. 127", 856.99m, new DateTime(2020, 3, 14, 12, 0, 0, 0, DateTimeKind.Utc), "as82340ddsdf80dsf81", "Active", null, 0m, null, "3y", new DateTime(2023, 3, 14, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("2dbf4106-9194-4125-8d4d-095e10cba4f4"), "Laptops", null, "asdfi230ser3jsadf012", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "94-39-90-DB-C1-B8", "Xiaomi", "Redmibook 13", "", "Sasamba Str. 23", 545m, new DateTime(2020, 12, 1, 12, 0, 0, 0, DateTimeKind.Utc), "as82312421sdaa180dsf81", "Active", null, 0m, null, "1y", new DateTime(2021, 12, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("65f245ae-81b6-425d-adaf-eb3ac1a2a5b9"), "Laptops", null, "1234asdasf929123asf11ads", new DateTime(2020, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc), "DF-64-62-7F-47-36", "Acer", "Aspire 5", "", "Kaliko Str. 127", 1123.21m, new DateTime(2020, 8, 21, 12, 0, 0, 0, DateTimeKind.Utc), "as82340ddsdf80dsf81", "Active", null, 112.56m, null, "3y", new DateTime(2023, 8, 21, 12, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Address", "City", "Country", "Department", "EmploymentDate", "FirstName", "Job", "LastName", "OfficeAddress", "Phone", "State", "TenantId", "UserId", "ZipCode" },
                values: new object[,]
                {
                    { new Guid("4c3c2c62-0ee8-4abb-93fa-d9bbf185d1d2"), "Sunnie Star Str.", "New-Carbon", "New USexico", "Software_Development", new DateTime(2021, 5, 22, 12, 0, 0, 0, DateTimeKind.Utc), "Kent", "Unity Developer", "Zet", "Kaliko Str. 127", "+380734098996", "Hemprane", null, null, "112522" },
                    { new Guid("74257e60-490f-472f-ae81-17c9518a7ee6"), "Big Guy Str.", "New-Popone", "New USexico", "Software_Development", new DateTime(2021, 4, 10, 12, 0, 0, 0, DateTimeKind.Utc), "Rose", "ASP.NET Core Developer", "Lee", "Kaliko Str. 127", "+380734098995", "Jorji", null, null, "129823" },
                    { new Guid("e5efeedd-207b-422c-8637-b2a4564d0737"), "Sad dick Str.", "New-Cockie", "New USexico", "Hardware_Development", new DateTime(2021, 5, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Nikkie", "Drones Developer", "Lol", "Kaliko Str. 127", "+380734098997", "Mranda", null, null, "542861" }
                });
        }
    }
}
