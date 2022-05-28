using LeaveManagement.Mvc.Models;
using LeaveManagement.Mvc.Services.Base;
using System.Threading.Tasks;

namespace LeaveManagement.Mvc.Contracts
{
    public interface ILeaveRequestService
    {
        Task<AdminLeaveRequestViewVM> GetAdminLeaveRequests();
        Task<EmployeeLeaveRequestViewVM> GetEmployeeLeaveRequests();
        Task<Response<int>> CreateLeaveRequest(CreateLeaveRequestVM request);
        Task<LeaveRequestVM> GetLeaveRequest(int id);
        Task DeleteLeaveRequest(int id);
        Task ApproveLeaveRequest(int id, bool approved);
    }
}
