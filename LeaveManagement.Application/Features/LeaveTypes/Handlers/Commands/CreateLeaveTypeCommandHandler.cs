using AutoMapper;
using LeaveManagement.Application.DTOs.LeaveType.Validators;
using LeaveManagement.Application.Exceptions;
using LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using LeaveManagement.Application.Contracts.Persitence;
using LeaveManagement.Domain;
using MediatR;

namespace LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
    public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateLeaveTypeCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveTypeDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CreateLeaveTypeDto, cancellationToken);

            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult);

            var leaveType = _mapper.Map<LeaveType>(request.CreateLeaveTypeDto);

            leaveType = await _unitOfWork.LeaveTypeRepository.Add(leaveType);

            await _unitOfWork.Save();

            return leaveType.Id;
        }
    }
}
