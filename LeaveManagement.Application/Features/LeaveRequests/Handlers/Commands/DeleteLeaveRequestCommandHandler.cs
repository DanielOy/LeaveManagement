using LeaveManagement.Application.Exceptions;
using LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using LeaveManagement.Application.Contracts.Persitence;
using LeaveManagement.Domain;
using MediatR;

namespace LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class DeleteLeaveRequestCommandHandler : IRequestHandler<DeleteLeaveRequestCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLeaveRequestCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _unitOfWork.LeaveRequestRepository.Get(request.Id);

            if (leaveRequest is null)
                throw new NotFoundException(nameof(LeaveRequest), request.Id);

            await _unitOfWork.LeaveRequestRepository.Delete(leaveRequest);

            await _unitOfWork.Save();

            return Unit.Value;
        }
    }
}
