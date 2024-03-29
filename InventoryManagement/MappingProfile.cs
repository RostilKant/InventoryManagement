﻿using System;
using AutoMapper;
using Entities.DataTransferObjects.Accessory;
using Entities.DataTransferObjects.Component;
using Entities.DataTransferObjects.Consumable;
using Entities.DataTransferObjects.Device;
using Entities.DataTransferObjects.Employee;
using Entities.DataTransferObjects.License;
using Entities.DataTransferObjects.User;
using Entities.IdentityModels;
using Entities.Models;

namespace InventoryManagement
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(c => c.FullName,
                    options =>
                        options.MapFrom(x => $"{x.FirstName} {x.LastName}"));

            CreateMap<EmployeeForCreationDto, Employee>();
            
            CreateMap<EmployeeForUpdateDto, Employee>();

            CreateMap<Device, DeviceDto>();
            CreateMap<DeviceForCreationDto, Device>();
            CreateMap<DeviceForUpdateDto, Device>();

            CreateMap<Accessory, AccessoryDto>();
            CreateMap<AccessoryForCreationDto, Accessory>();
            CreateMap<AccessoryForUpdateDto, Accessory>();
            
            CreateMap<Component, ComponentDto>();
            CreateMap<ComponentForCreationDto, Component>();
            CreateMap<ComponentForUpdateDto, Component>();
            
            CreateMap<Consumable, ConsumableDto>();
            CreateMap<ConsumableForCreationDto, Consumable>();
            CreateMap<ConsumableForUpdateDto, Consumable>();
            
            CreateMap<License, LicenseDto>();
            CreateMap<LicenseForCreationDto, License>();
            CreateMap<LicenseForUpdateDto, License>();

            CreateMap<UserForRegistrationDto, User>()
                .AfterMap((src, dest) =>
                {
                    dest.Id = Guid.NewGuid();
                    dest.UserName = dest.Id.ToString();
                });
            
            CreateMap<UserForUpdateDto, User>();
        }
    }
}