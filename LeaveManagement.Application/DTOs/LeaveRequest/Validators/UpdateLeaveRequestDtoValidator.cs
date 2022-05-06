using FluentValidation;
using LeaveManagement.Application.Persitence.Contract;

namespace LeaveManagement.Application.DTOs.LeaveRequest.Validators
{
    public class UpdateLeaveRequestDtoValidator : AbstractValidator<UpdateLeaveRequestDto>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public UpdateLeaveRequestDtoValidator(ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;

            Include(new IBasicLeaveRequestDtoValidator(_leaveRequestRepository));

            RuleFor(p => p.Id)
               .NotNull().WithMessage("{PropertyName} is required");
        }
    }
}
