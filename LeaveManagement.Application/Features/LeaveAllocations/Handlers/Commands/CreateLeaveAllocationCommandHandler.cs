using AutoMapper;
using LeaveManagement.Application.DTOs.LeaveAllocation.Validators;
using LeaveManagement.Application.Exceptions;
using LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using LeaveManagement.Application.Contracts.Persitence;
using LeaveManagement.Domain;
using MediatR;
using LeaveManagement.Application.Contracts.Identity;

namespace LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CreateLeaveAllocationCommandHandler(IMapper mapper, IUserService userService, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveAllocationDtoValidator(_unitOfWork.LeaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request.CreateLeaveAllocationDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

            var allocations = new List<LeaveAllocation>();
            var leaveType = await _unitOfWork.LeaveTypeRepository.Get(request.CreateLeaveAllocationDto.LeaveTypeId);
            var employees = await _userService.GetEmployees();
            int period = DateTime.Now.Year;

            foreach (var employee in employees)
            {
                if (!await _unitOfWork.LeaveAllocationRepository.AllocationExists(employee.Id, leaveType.Id, period))
                {
                    allocations.Add(new LeaveAllocation
                    {
                        EmployeeId = employee.Id,
                        LeaveTypeId = leaveType.Id,
                        NumberOfDays = leaveType.DefaultDays,
                        Period = period
                    });
                }
            }

            await _unitOfWork.LeaveAllocationRepository.AddAllocations(allocations);

            await _unitOfWork.Save();

            return 1;
        }
    }
}
