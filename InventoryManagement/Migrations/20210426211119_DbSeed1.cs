using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagement.Migrations
{
    public partial class DbSeed1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Warranty",
                table: "Devices",
                type: "text",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "DeviceId", "Category", "EmployeeId", "Imei", "LastUpdateDate", "MacAddress", "Manufacturer", "Model", "Notes", "OfficeAddress", "PurchaseCost", "PurchaseDate", "Serial", "Status", "UpdateCost", "Warranty", "WarrantyExpires" },
                values: new object[,]
                {
                    { new Guid("65f245ae-81b6-425d-adaf-eb3ac1a2a5b9"), 1, null, "1234asdasf929123asf11ads", new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "DF-64-62-7F-47-36", "Acer", "Aspire 5", "", "Kaliko Str. 127", 1123.21m, new DateTime(2020, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "as82340ddsdf80dsf81", 4, 112.56m, "3y", new DateTime(2023, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("2dbf4106-9194-4125-8d4d-095e10cba4f4"), 1, null, "asdfi230ser3jsadf012", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "94-39-90-DB-C1-B8", "Xiaomi", "Redmibook 13", "", "Sasamba Str. 23", 545m, new DateTime(2020, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "as82312421sdaa180dsf81", 4, 0m, "1y", new DateTime(2021, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("2b7efe32-3e84-4e46-8a8d-af5f757a0413"), 1, null, "q39450ifjsdgsjdgjs12342hd", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AF-55-AF-35-CD-DF", "Asus", "Vivobook 14", "", "Kaliko Str. 127", 856.99m, new DateTime(2020, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "as82340ddsdf80dsf81", 4, 0m, "3y", new DateTime(2023, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Address", "City", "Country", "Department", "EmploymentDate", "FirstName", "Job", "LastName", "OfficeAddress", "Phone", "State", "ZipCode" },
                values: new object[,]
                {
                    { new Guid("74257e60-490f-472f-ae81-17c9518a7ee6"), "Big Guy Str.", "New-Popone", "New USexico", 0, new DateTime(2021, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rose", "ASP.NET Core Developer", "Lee", "Kaliko Str. 127", "+380734098995", "Jorji", "129823" },
                    { new Guid("4c3c2c62-0ee8-4abb-93fa-d9bbf185d1d2"), "Sunnie Star Str.", "New-Carbon", "New USexico", 0, new DateTime(2021, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kent", "Unity Developer", "Zet", "Kaliko Str. 127", "+380734098996", "Hemprane", "112522" },
                    { new Guid("e5efeedd-207b-422c-8637-b2a4564d0737"), "Sad dick Str.", "New-Cockie", "New USexico", 3, new DateTime(2021, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nikkie", "Drones Developer", "Lol", "Kaliko Str. 127", "+380734098997", "Mranda", "542861" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "Warranty",
                table: "Devices",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
