using LeaveManagement.Application.Contracts.Persitence;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace LeaveManagement.Application.UnitTests.Mocks
{
    using LeaveManagement.Domain;

    public static class MockLeaveRequestRepository
    {
        public static Mock<ILeaveRequestRepository> GetLeaveRequestRepository()
        {
            var LeaveRequests = new List<LeaveRequest>
            {
                new LeaveRequest
                {
                    Id = 1,
                    LeaveTypeId=1,
                    Approved=true
                },
                new LeaveRequest
                {
                    Id = 1,
                    LeaveTypeId=2,
                    Approved=true
                }
            };

            var mockRepo = new Mock<ILeaveRequestRepository>();

            mockRepo
                .Setup(x => x.GetAll())
                .ReturnsAsync(LeaveRequests);

            mockRepo
                .Setup(x => x.GetLeaveRequestsWithDetails())
                .ReturnsAsync(LeaveRequests);

            mockRepo
                .Setup(x => x.Get(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    return LeaveRequests.FirstOrDefault(x => x.Id == id);
                });

            mockRepo
                .Setup(x => x.GetLeaveRequestWithDetails(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    return LeaveRequests.FirstOrDefault(x => x.Id == id);
                });

            mockRepo
                .Setup(x => x.Add(It.IsAny<LeaveRequest>()))
                .ReturnsAsync((LeaveRequest LeaveRequest) =>
                {
                    LeaveRequests.Add(LeaveRequest);
                    return LeaveRequest;
                });

            mockRepo
                .Setup(x => x.Delete(It.IsAny<LeaveRequest>()))
                .Callback((LeaveRequest LeaveRequest) =>
                {
                    LeaveRequests.Remove(LeaveRequest);
                });

            return mockRepo;
        }
    }
}
