using AutoMapper;
using LeaveManagement.Application.Contracts.Persitence;
using LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands;
using LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using LeaveManagement.Application.Profiles;
using LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeaveManagement.Application.UnitTests.LeaveRequests.Commands
{
    public class DeleteLeaveRequestCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public DeleteLeaveRequestCommandHandlerTests()
        {
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
        }

        [Test]
        public async Task Valid_LeaveRequest_Deleted()
        {
            //Arrange
            var handler = new DeleteLeaveRequestCommandHandler(_mockUnitOfWork.Object);
            var request = new DeleteLeaveRequestCommand() { Id = 1 };

            //Act
            var result = await handler.Handle(request, CancellationToken.None);

            var LeaveRequests = await _mockUnitOfWork.Object.LeaveRequestRepository.GetAll();

            //Assert
            LeaveRequests.Count().ShouldBe(1);
        }

        [Test]
        public async Task Invalid_LeaveRequest_Deleted()
        {
            //Arrange
            var handler = new DeleteLeaveRequestCommandHandler(_mockUnitOfWork.Object);
            var request = new DeleteLeaveRequestCommand() { Id = 3 };

            //Act
            var exception = await Should.ThrowAsync<ApplicationException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });


            //Assert
            exception.ShouldNotBeNull();

            var LeaveRequests = await _mockUnitOfWork.Object.LeaveRequestRepository.GetAll();
            LeaveRequests.Count().ShouldBe(2);
        }
    }
}
