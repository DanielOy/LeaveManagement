using LeaveManagement.Application.Exceptions;
using LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using LeaveManagement.Application.Contracts.Persitence;
using LeaveManagement.Domain;
using MediatR;

namespace LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
    public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLeaveTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var leaveType = await _unitOfWork.LeaveTypeRepository.Get(request.Id);

            if (leaveType is null)
                throw new ApplicationException(nameof(LeaveType), new NotFoundException(nameof(leaveType), request.Id));

            await _unitOfWork.LeaveTypeRepository.Delete(leaveType);

            await _unitOfWork.Save();

            return Unit.Value;
        }
    }
}
