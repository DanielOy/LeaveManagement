using FluentValidation;
using LeaveManagement.Application.Contracts.Persitence;

namespace LeaveManagement.Application.DTOs.LeaveAllocation.Validators
{
    public class UpdateLeaveAllocationDtoValidator : AbstractValidator<UpdateLeaveAllocationDto>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;

        public UpdateLeaveAllocationDtoValidator(ILeaveAllocationRepository leaveAllocationRepository)
        {
            _leaveAllocationRepository = leaveAllocationRepository;

            Include(new ILeaveAllocationDtoValidator(_leaveAllocationRepository));

            RuleFor(p => p.Id)
               .NotNull().WithMessage("{PropertyName} is required");
        }
    }
}
