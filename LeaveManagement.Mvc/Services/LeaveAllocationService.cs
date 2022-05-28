using AutoMapper;
using LeaveManagement.Mvc.Contracts;
using LeaveManagement.Mvc.Services.Base;
using System;
using System.Threading.Tasks;

namespace LeaveManagement.Mvc.Services
{
    public class LeaveAllocationService : BaseHttpService, ILeaveAllocationService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IClient _httpClient;

        public LeaveAllocationService(ILocalStorageService localStorageService, IClient client) : base(localStorageService, client)
        {
            _localStorageService = localStorageService;
            _httpClient = client;
        }

        public async Task<Response<int>> CreateLeaveAllocations(int leaveTypeId)
        {
            try
            {
                var createLeaveAllocation = new CreateLeaveAllocationDto { LeaveTypeId = leaveTypeId };
                AddBearerToken();

                await _client.LeaveAllocationPOSTAsync(createLeaveAllocation);

                return new Response<int>() { Success = true };
            }
            catch (ApiException ex)
            {
                return ConvertApiException<int>(ex);
            }
        }
    }
}
