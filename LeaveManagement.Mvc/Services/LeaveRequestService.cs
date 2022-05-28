using AutoMapper;
using LeaveManagement.Mvc.Contracts;
using LeaveManagement.Mvc.Models;
using LeaveManagement.Mvc.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Mvc.Services
{
    public class LeaveRequestService : BaseHttpService, ILeaveRequestService
    {
        private readonly ILocalStorageService _storageService;
        private readonly IMapper _mapper;

        public LeaveRequestService(ILocalStorageService localStorage, IClient client, IMapper mapper, ILocalStorageService storageService) : base(localStorage, client)
        {
            _mapper = mapper;
            _storageService = storageService;
        }

        public async Task ApproveLeaveRequest(int id, bool approved)
        {
            AddBearerToken();
            var request = new ChangeLeaveRequestApprovalDto { Id = id, Approved = approved };
            await _client.ChangeapprovalAsync(request);
        }

        public async Task<Response<int>> CreateLeaveRequest(CreateLeaveRequestVM request)
        {
            try
            {
                var response = new Response<int>();
                var createLeaveRequest = _mapper.Map<CreateLeaveRequestDto>(request);
                AddBearerToken();
                var apiReponse = await _client.LeaveRequestPOSTAsync(createLeaveRequest);

                response.Success = true;
                response.Data = apiReponse;
                return response;

            }
            catch (ApiException ex)
            {
                return ConvertApiException<int>(ex);
            }
        }

        public Task DeleteLeaveRequest(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AdminLeaveRequestViewVM> GetAdminLeaveRequests()
        {
            AddBearerToken();
            var leaveRequests = await _client.LeaveRequestAllAsync(isLoggedInUser: false);
            var model = new AdminLeaveRequestViewVM
            {
                TotalRequests = leaveRequests.Count,
                ApprovedRequests = leaveRequests.Count(x => x.Approved == true),
                RejectedRequests = leaveRequests.Count(x => x.Approved == false),
                PendingRequests = leaveRequests.Count(x => x.Approved == null),
                LeaveRequests = _mapper.Map<List<LeaveRequestVM>>(leaveRequests)
            };

            return model;
        }

        public async Task<EmployeeLeaveRequestViewVM> GetEmployeeLeaveRequests()
        {
            AddBearerToken();

            var requests = await _client.LeaveRequestAllAsync(true);
            var allocations = await _client.LeaveAllocationAllAsync(true);

            var model = new EmployeeLeaveRequestViewVM
            {
                LeaveAllocations = _mapper.Map<List<LeaveAllocationVM>>(allocations),
                LeaveRequests = _mapper.Map<List<LeaveRequestVM>>(requests)
            };

            return model;
        }

        public async Task<LeaveRequestVM> GetLeaveRequest(int id)
        {
            AddBearerToken();

            var request = await _client.LeaveRequestGETAsync(id);

            return _mapper.Map<LeaveRequestVM>(request);
        }
    }
}
