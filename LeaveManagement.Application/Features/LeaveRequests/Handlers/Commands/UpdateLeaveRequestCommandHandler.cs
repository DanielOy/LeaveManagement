using AutoMapper;
using LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using LeaveManagement.Application.Exceptions;
using LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using LeaveManagement.Application.Contracts.Persitence;
using MediatR;
using LeaveManagement.Domain;

namespace LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLeaveRequestCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            LeaveRequest leaveRequest;

            if (request.LeaveRequestDto != null)
            {
                var validator = new UpdateLeaveRequestDtoValidator(_unitOfWork.LeaveRequestRepository);
                var validationResult = await validator.ValidateAsync(request.LeaveRequestDto, cancellationToken);

                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult);

                leaveRequest = await _unitOfWork.LeaveRequestRepository.Get(request.LeaveRequestDto.Id);

                if (leaveRequest is null)
                    throw new NotFoundException(nameof(leaveRequest), request.LeaveRequestDto.Id);

                _mapper.Map(request.LeaveRequestDto, leaveRequest);

                await _unitOfWork.LeaveRequestRepository.Update(leaveRequest);

                await _unitOfWork.Save();
            }
            else if (request.ChangeLeaveRequestApprovalDto != null)
            {
                leaveRequest = await _unitOfWork.LeaveRequestRepository.Get(request.ChangeLeaveRequestApprovalDto.Id);

                if (leaveRequest is null)
                    throw new NotFoundException(nameof(leaveRequest), request.LeaveRequestDto.Id);

                await _unitOfWork.LeaveRequestRepository.ChangeApprovalStatus(leaveRequest, request.ChangeLeaveRequestApprovalDto.Approved);
                if (request.ChangeLeaveRequestApprovalDto.Approved == true)
                {
                    var allocation = await _unitOfWork.LeaveAllocationRepository.GetUserAllocations(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId);
                    int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;

                    allocation.NumberOfDays -= daysRequested;

                    await _unitOfWork.LeaveAllocationRepository.Update(allocation);
                }

                await _unitOfWork.Save();
            }
            else
            {
                throw new ApplicationException($"Value required {nameof(request.LeaveRequestDto)} or {nameof(request.ChangeLeaveRequestApprovalDto)}");
            }

            return Unit.Value;
        }
    }
}
