using System;
using Entities.Enums;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.HasData(
                new Device
                {
                    Id = new Guid("65f245ae-81b6-425d-adaf-eb3ac1a2a5b9"),
                    Model = "Aspire 5",
                    Serial = "as82340ddsdf80dsf81",
                    Category = DeviceCategory.Laptops,
                    Status = AssetStatus.Active,
                    Manufacturer = "Acer",
                    OfficeAddress = "Kaliko Str. 127",
                    PurchaseCost = (decimal) 1123.21,
                    PurchaseDate = new DateTime(2020, 8, 21, 12, 0, 0, DateTimeKind.Utc),
                    UpdateCost = (decimal) 112.56,
                    LastUpdateDate = new DateTime(2020, 11, 1, 12, 0, 0, DateTimeKind.Utc),
                    Warranty = "3y",
                    WarrantyExpires = new DateTime(2023, 8, 21, 12, 0, 0, DateTimeKind.Utc),
                    Imei = "1234asdasf929123asf11ads",
                    MacAddress = "DF-64-62-7F-47-36",
                    Notes = ""
                },
                new Device
                {
                    Id = new Guid("2dbf4106-9194-4125-8d4d-095e10cba4f4"),
                    Model = "Redmibook 13",
                    Serial = "as82312421sdaa180dsf81",
                    Category = DeviceCategory.Laptops,
                    Status = AssetStatus.Active,
                    Manufacturer = "Xiaomi",
                    OfficeAddress = "Sasamba Str. 23",
                    PurchaseCost = 545,
                    PurchaseDate = new DateTime(2020, 12, 1, 12, 0, 0, DateTimeKind.Utc),
                    Warranty = "1y",
                    WarrantyExpires = new DateTime(2021, 12, 1, 12, 0, 0, DateTimeKind.Utc),
                    Imei = "asdfi230ser3jsadf012",
                    MacAddress = "94-39-90-DB-C1-B8",
                    Notes = ""
                },
                new Device
                {
                    Id = new Guid("2b7efe32-3e84-4e46-8a8d-af5f757a0413"),
                    Model = "Vivobook 14",
                    Serial = "as82340ddsdf80dsf81",
                    Category = DeviceCategory.Laptops,
                    Status = AssetStatus.Active,
                    Manufacturer = "Asus",
                    OfficeAddress = "Kaliko Str. 127",
                    PurchaseCost = (decimal) 856.99,
                    PurchaseDate = new DateTime(2020, 3, 14, 12, 0, 0, DateTimeKind.Utc),
                    Warranty = "3y",
                    WarrantyExpires = new DateTime(2023, 3, 14, 12, 0, 0, DateTimeKind.Utc),
                    Imei = "q39450ifjsdgsjdgjs12342hd",
                    MacAddress = "AF-55-AF-35-CD-DF",
                    Notes = ""
                }
            );
        }
    }
}