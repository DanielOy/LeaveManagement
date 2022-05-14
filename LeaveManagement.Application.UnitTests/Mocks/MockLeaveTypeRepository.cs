using LeaveManagement.Application.Contracts.Persitence;
using LeaveManagement.Domain;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace LeaveManagement.Application.UnitTests.Mocks
{
    public static class MockLeaveTypeRepository
    {
        public static Mock<ILeaveTypeRepository> GetLeaveTypeRepository()
        {
            var leaveTypes = new List<LeaveType>
            {
                new LeaveType
                {
                    Id=1,
                    DefaultDays=10,
                    Name="Test Vacation"
                },
                new LeaveType
                {
                    Id=2,
                    DefaultDays=15,
                    Name="Test Sick"
                }
            };

            var mockRepo = new Mock<ILeaveTypeRepository>();

            mockRepo
                .Setup(x => x.GetAll())
                .ReturnsAsync(leaveTypes);

            mockRepo
                .Setup(x => x.Get(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    return leaveTypes.FirstOrDefault(x => x.Id == id);
                });

            mockRepo
                .Setup(x => x.Add(It.IsAny<LeaveType>()))
                .ReturnsAsync((LeaveType leaveType) =>
                {
                    leaveTypes.Add(leaveType);
                    return leaveType;
                });

            mockRepo
                .Setup(x => x.Delete(It.IsAny<LeaveType>()))
                .Callback((LeaveType leaveType) =>
                {
                    leaveTypes.Remove(leaveType);
                });

            mockRepo
                .Setup(x => x.Exists(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    return leaveTypes.Any(x => x.Id == id);
                });

            return mockRepo;
        }
    }
}
