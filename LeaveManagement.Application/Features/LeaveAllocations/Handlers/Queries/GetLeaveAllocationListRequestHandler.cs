﻿using AutoMapper;
using LeaveManagement.Application.DTOs.LeaveAllocation;
using LeaveManagement.Application.Features.LeaveAllocations.Requests.Queries;
using LeaveManagement.Application.Contracts.Persitence;
using MediatR;
using Microsoft.AspNetCore.Http;
using LeaveManagement.Application.Contracts.Identity;
using LeaveManagement.Domain;
using LeaveManagement.Application.Constants;

namespace LeaveManagement.Application.Features.LeaveAllocations.Handlers.Queries
{
    public class GetLeaveAllocationListRequestHandler : IRequestHandler<GetLeaveAllocationListRequest, List<LeaveAllocationDto>>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserService _userService;

        public GetLeaveAllocationListRequestHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper, IHttpContextAccessor httpContext, IUserService userService)
        {
            _leaveAllocationRepository = leaveAllocationRepository;
            _mapper = mapper;
            _httpContext = httpContext;
            _userService = userService;
        }

        public async Task<List<LeaveAllocationDto>> Handle(GetLeaveAllocationListRequest request, CancellationToken cancellationToken)
        {
            var leaveAllocations = new List<LeaveAllocation>();
            var allocations = new List<LeaveAllocationDto>();

            if (request.IsLoggedUser)
            {
                var userId = _httpContext.HttpContext.User.FindFirst(x => x.Type == CustomClaimTypes.Uid)?.Value;
                var employee = await _userService.GetEmployee(userId);

                leaveAllocations = await _leaveAllocationRepository.GetLeaveAllocationsWithDetails(userId);
                allocations = _mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);


                foreach (var allocation in allocations)
                    allocation.Employee = employee;
            }
            else
            {
                leaveAllocations = await _leaveAllocationRepository.GetLeaveAllocationsWithDetails();
                allocations = _mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);

                foreach (var allocation in allocations)
                    allocation.Employee = await _userService.GetEmployee(allocation.EmployeeId);
            }

            return allocations;
        }
    }
}
