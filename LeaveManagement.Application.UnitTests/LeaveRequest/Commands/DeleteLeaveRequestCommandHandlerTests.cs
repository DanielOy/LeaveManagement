using AutoMapper;
using LeaveManagement.Application.Contracts.Persitence;
using LeaveManagement.Application.DTOs.LeaveRequest;
using LeaveManagement.Application.Exceptions;
using LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands;
using LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using LeaveManagement.Application.Profiles;
using LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeaveManagement.Application.UnitTests.LeaveRequests.Commands
{
    public class DeleteLeaveRequestCommandHandlerTests
    {
        private readonly Mock<ILeaveRequestRepository> _mockRepo;

        public DeleteLeaveRequestCommandHandlerTests()
        {
            _mockRepo = MockLeaveRequestRepository.GetLeaveRequestRepository();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
        }

        [Test]
        public async Task Valid_LeaveRequest_Deleted()
        {
            //Arrange
            var handler = new DeleteLeaveRequestCommandHandler(_mockRepo.Object);
            var request = new DeleteLeaveRequestCommand() { Id = 1 };

            //Act
            var result = await handler.Handle(request, CancellationToken.None);

            var LeaveRequests = await _mockRepo.Object.GetAll();

            //Assert
            LeaveRequests.Count().ShouldBe(1);
        }

        [Test]
        public async Task Invalid_LeaveRequest_Deleted()
        {
            //Arrange
            var handler = new DeleteLeaveRequestCommandHandler(_mockRepo.Object);
            var request = new DeleteLeaveRequestCommand() { Id = 3 };

            //Act
            var exception = await Should.ThrowAsync<ApplicationException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });


            //Assert
            exception.ShouldNotBeNull();

            var LeaveRequests = await _mockRepo.Object.GetAll();
            LeaveRequests.Count().ShouldBe(2);
        }
    }
}
