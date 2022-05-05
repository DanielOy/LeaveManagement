using LeaveManagement.Application.DTOs.Common;

namespace LeaveManagement.Application.DTOs.LeaveAllocation
{
    public class UpdateLeaveAllocationDto:BaseDto
    {
        public int NumberOfDays { get; set; }
        public int LeaveTypeId { get; set; }
        public int Period { get; set; }
    }
}
