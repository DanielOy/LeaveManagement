using FluentValidation;
using LeaveManagement.Application.Contracts.Persitence;

namespace LeaveManagement.Application.DTOs.LeaveRequest.Validators
{
    public class IBasicLeaveRequestDtoValidator : AbstractValidator<IBasicLeaveRequestDto>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public IBasicLeaveRequestDtoValidator(ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;

            RuleFor(p => p.StartDate)
                .LessThan(p => p.EndDate).WithMessage("{PropertyName} must be before {ComparisonValue}");

            RuleFor(p => p.EndDate)
                .GreaterThan(p => p.StartDate).WithMessage("{PropertyName} must be after {ComparisonValue}");

            RuleFor(p => p.LeaveTypeId)
                .GreaterThan(0)
                .MustAsync(async (id, token) =>
                {
                    var leaveTypeExists = await _leaveRequestRepository.Exists(id);
                    return leaveTypeExists;
                }).WithMessage("{PropertyName} doesn't exist");
        }
    }
}