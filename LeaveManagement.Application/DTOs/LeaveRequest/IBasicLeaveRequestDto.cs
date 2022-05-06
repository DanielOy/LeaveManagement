namespace LeaveManagement.Application.DTOs.LeaveRequest
{
    public interface IBasicLeaveRequestDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int LeaveTypeId { get; set; }
    }
}
