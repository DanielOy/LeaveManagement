using AutoMapper;
using LeaveManagement.Application.DTOs.LeaveRequest;
using LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using LeaveManagement.Application.Contracts.Persitence;
using MediatR;
using Microsoft.AspNetCore.Http;
using LeaveManagement.Application.Contracts.Identity;
using LeaveManagement.Domain;
using LeaveManagement.Application.Constants;

namespace LeaveManagement.Application.Features.LeaveRequests.Handlers.Queries
{
    public class GetLeaveRequestListRequestHandler : IRequestHandler<GetLeaveRequestListRequest, List<LeaveRequestListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserService _userService;

        public GetLeaveRequestListRequestHandler(IMapper mapper, IHttpContextAccessor httpContext, IUserService userService, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _httpContext = httpContext;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<LeaveRequestListDto>> Handle(GetLeaveRequestListRequest request, CancellationToken cancellationToken)
        {
            var leaveRequests = new List<LeaveRequest>();
            var requests = new List<LeaveRequestListDto>();

            if (request.IsLoggedInUser)
            {
                var userId = _httpContext.HttpContext.User.FindFirst(q => q.Type == CustomClaimTypes.Uid)?.Value;
                leaveRequests = await _unitOfWork.LeaveRequestRepository.GetLeaveRequestsWithDetails(userId);

                var employee = await _userService.GetEmployee(userId);
                requests = _mapper.Map<List<LeaveRequestListDto>>(leaveRequests);

                foreach (var req in requests)
                    req.Employee = employee;
            }
            else
            {
                leaveRequests = await _unitOfWork.LeaveRequestRepository.GetLeaveRequestsWithDetails();
                requests = _mapper.Map<List<LeaveRequestListDto>>(leaveRequests);

                foreach (var req in requests)
                {
                    req.Employee = await _userService.GetEmployee(req.RequestingEmployeeId);
                }
            }

            return requests;
        }
    }
}
