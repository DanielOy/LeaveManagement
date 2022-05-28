using AutoMapper;
using LeaveManagement.Application.DTOs.LeaveAllocation.Validators;
using LeaveManagement.Application.Exceptions;
using LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using LeaveManagement.Application.Contracts.Persitence;
using MediatR;

namespace LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
    public class UpdateLeaveAllocationCommandHandler : IRequestHandler<UpdateLeaveAllocationCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLeaveAllocationCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateLeaveAllocationDtoValidator(_unitOfWork.LeaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request.UpdateLeaveAllocationDto, cancellationToken);
            var types = await _unitOfWork.LeaveTypeRepository.GetAll();
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

            var leaveAllocation = await _unitOfWork.LeaveAllocationRepository.Get(request.UpdateLeaveAllocationDto.Id);

            _mapper.Map(request.UpdateLeaveAllocationDto, leaveAllocation);

            await _unitOfWork.LeaveAllocationRepository.Update(leaveAllocation);

            await _unitOfWork.Save();

            return Unit.Value;
        }
    }
}
