using AutoMapper;
using LeaveManagement.Mvc.Models;
using LeaveManagement.Mvc.Services.Base;

namespace LeaveManagement.Mvc
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateLeaveTypeDto, CreateLeaveTypeVM>().ReverseMap();
            CreateMap<CreateLeaveRequestDto, CreateLeaveRequestVM>().ReverseMap();
            CreateMap<LeaveRequestDto, LeaveRequestVM>()
                .ForMember(q => q.DateRequested, options => options.MapFrom(origin => origin.DateRequested.Date))
                .ForMember(q => q.StartDate, options => options.MapFrom(origin => origin.StartDate.Date))
                .ForMember(q => q.EndDate, options => options.MapFrom(origin => origin.EndDate.Date))
                .ReverseMap();
            CreateMap<LeaveRequestListDto, LeaveRequestVM>()
            .ForMember(q => q.DateRequested, options => options.MapFrom(origin => origin.DateRequested.Date))
            .ForMember(q => q.StartDate, options => options.MapFrom(origin => origin.StartDate.Date))
            .ForMember(q => q.EndDate, options => options.MapFrom(origin => origin.EndDate.Date))
            .ReverseMap();
            CreateMap<LeaveTypeDto, LeaveTypeVM>().ReverseMap();
            CreateMap<LeaveAllocationDto, LeaveAllocationVM>().ReverseMap();
            CreateMap<RegistrationRequest, RegisterVM>().ReverseMap();
            CreateMap<EmployeeVM, Employee>().ReverseMap();
        }
    }
}
