using LeaveManagement.Mvc.Services.Base;
using System.Threading.Tasks;

namespace LeaveManagement.Mvc.Contracts
{
    public interface ILeaveAllocationService
    {
        Task<Response<int>> CreateLeaveAllocations(int leaveTypeId);
    }
}
