using System;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.Device;
using Entities.DataTransferObjects.Employee;
using Entities.Enums;
using Entities.Models;
using Microsoft.OpenApi.Extensions;

namespace InventoryManagement
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(c => c.Department,
                    options =>
                        options.MapFrom(x => x.Department.ToString().Replace('_', ' ')))
                .ForMember(c => c.FullName,
                    options =>
                        options.MapFrom(x => $"{x.FirstName} {x.LastName}"));
            
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<EmployeeForUpdateDto, Employee>();

            CreateMap<Device, DeviceDto>()
                .ForMember(d => d.Category, 
                    memberOptions => 
                        memberOptions.MapFrom(x => Enum.GetName(x.Category)))
                .ForMember(d => d.Status, 
                    memberOptions => 
                        memberOptions.MapFrom(x => Enum.GetName(x.Status))); 
            CreateMap<DeviceForCreationDto, Device>();
            CreateMap<DeviceForUpdateDto, Device>();
        }
    }
}