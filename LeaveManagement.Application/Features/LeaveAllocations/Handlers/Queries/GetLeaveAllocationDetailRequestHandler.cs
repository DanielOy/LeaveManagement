using AutoMapper;
using LeaveManagement.Application.DTOs.LeaveAllocation;
using LeaveManagement.Application.Features.LeaveAllocations.Requests.Queries;
using LeaveManagement.Application.Contracts.Persitence;
using MediatR;
using LeaveManagement.Application.Contracts.Identity;
using LeaveManagement.Application.Exceptions;

namespace LeaveManagement.Application.Features.LeaveAllocations.Handlers.Queries
{
    public class GetLeaveAllocationDetailRequestHandler : IRequestHandler<GetLeaveAllocationDetailRequest, LeaveAllocationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public GetLeaveAllocationDetailRequestHandler(IMapper mapper, IUnitOfWork unitOfWork, IUserService userService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<LeaveAllocationDto> Handle(GetLeaveAllocationDetailRequest request, CancellationToken cancellationToken)
        {
            var leaveAllocation = await _unitOfWork.LeaveAllocationRepository.GetLeaveAllocationWithDetails(request.Id);

            if (leaveAllocation is null)
                throw new NotFoundException(nameof(leaveAllocation), request.Id);

            var leaveAllocationDto = _mapper.Map<LeaveAllocationDto>(leaveAllocation);
            leaveAllocationDto.Employee = await _userService.GetEmployee(leaveAllocation.EmployeeId);
           
            return leaveAllocationDto;
        }
    }
}
