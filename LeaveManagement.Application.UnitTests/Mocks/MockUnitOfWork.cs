using LeaveManagement.Application.Contracts.Persitence;
using Moq;

namespace LeaveManagement.Application.UnitTests.Mocks
{
    public static class MockUnitOfWork
    {
        public static Mock<IUnitOfWork> GetUnitOfWork()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockLeaveType = MockLeaveTypeRepository.GetLeaveTypeRepository();
            var mockLeaveRequest = MockLeaveRequestRepository.GetLeaveRequestRepository();
            var mockLeaveAllocation = MockLeaveAllocationRepository.GetLeaveAllocationRepository();

            mockUnitOfWork.Setup(x => x.LeaveTypeRepository).Returns(mockLeaveType.Object);
            mockUnitOfWork.Setup(x => x.LeaveRequestRepository).Returns(mockLeaveRequest.Object);
            mockUnitOfWork.Setup(x => x.LeaveAllocationRepository).Returns(mockLeaveAllocation.Object);

            return mockUnitOfWork;
        }
    }
}
