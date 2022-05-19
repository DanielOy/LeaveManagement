using LeaveManagement.Mvc.Models;
using LeaveManagement.Mvc.Services.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveManagement.Mvc.Contracts
{
    public interface ILeaveTypeService
    {
        Task<List<LeaveTypeVM>> GetLeaveTypes();
        Task<LeaveTypeVM> GetLeaveTypeDetails(int id);
        Task<Response<int>> CreateLeaveType(CreateLeaveTypeVM leaveType);
        Task<Response<int>> UpdateLeaveType(LeaveTypeVM leaveType);
        Task<Response<int>> DeleteLeaveType(int id);
    }
}
