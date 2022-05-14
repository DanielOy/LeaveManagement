using LeaveManagement.Application.Contracts.Persitence;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace LeaveManagement.Application.UnitTests.Mocks
{
    using LeaveManagement.Domain;

    public static class MockLeaveAllocationRepository
    {
        public static Mock<ILeaveAllocationRepository> GetLeaveAllocationRepository()
        {
            var leaveAllocations = new List<LeaveAllocation>
            {
                new LeaveAllocation
                {
                   Id = 1,
                   LeaveTypeId=1,
                   NumberOfDays=1,
                   Period=2022
                },
                new LeaveAllocation
                {
                   Id = 2,
                   LeaveTypeId=2,
                   NumberOfDays=2,
                   Period=2022
                }
            };

            var mockRepo = new Mock<ILeaveAllocationRepository>();

            mockRepo
                .Setup(x => x.GetAll())
                .ReturnsAsync(leaveAllocations);

            mockRepo
                .Setup(x => x.GetLeaveAllocationsWithDetails())
                .ReturnsAsync(leaveAllocations);

            mockRepo
                .Setup(x => x.Get(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    return leaveAllocations.FirstOrDefault(x => x.Id == id);
                });

            mockRepo
                .Setup(x => x.GetLeaveAllocationWithDetails(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    return leaveAllocations.FirstOrDefault(x => x.Id == id);
                });

            mockRepo
                .Setup(x => x.Add(It.IsAny<LeaveAllocation>()))
                .ReturnsAsync((LeaveAllocation leaveAllocation) =>
                {
                    leaveAllocations.Add(leaveAllocation);
                    return leaveAllocation;
                });

            mockRepo
                .Setup(x => x.Delete(It.IsAny<LeaveAllocation>()))
                .Callback((LeaveAllocation leaveAllocation) =>
                {
                    leaveAllocations.Remove(leaveAllocation);
                });

            return mockRepo;
        }
    }
}
