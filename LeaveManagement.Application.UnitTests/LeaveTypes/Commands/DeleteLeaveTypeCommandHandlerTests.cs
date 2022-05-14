using AutoMapper;
using LeaveManagement.Application.Contracts.Persitence;
using LeaveManagement.Application.DTOs.LeaveType;
using LeaveManagement.Application.Exceptions;
using LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands;
using LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
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

namespace LeaveManagement.Application.UnitTests.LeaveTypes.Commands
{
    public class DeleteLeaveTypeCommandHandlerTests
    {
        private readonly Mock<ILeaveTypeRepository> _mockRepo;

        public DeleteLeaveTypeCommandHandlerTests()
        {
            _mockRepo = MockLeaveTypeRepository.GetLeaveTypeRepository();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
        }

        [Test]
        public async Task Valid_LeaveType_Deleted()
        {
            //Arrange
            var handler = new DeleteLeaveTypeCommandHandler(_mockRepo.Object);
            var request = new DeleteLeaveTypeCommand() { Id = 1 };

            //Act
            var result = await handler.Handle(request, CancellationToken.None);

            var leaveTypes = await _mockRepo.Object.GetAll();

            //Assert
            leaveTypes.Count().ShouldBe(1);
        }

        [Test]
        public async Task Invalid_LeaveType_Deleted()
        {
            //Arrange
            var handler = new DeleteLeaveTypeCommandHandler(_mockRepo.Object);
            var request = new DeleteLeaveTypeCommand() { Id = 3 };

            //Act
            var exception = await Should.ThrowAsync<ApplicationException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });


            //Assert
            exception.ShouldNotBeNull();

            var leaveTypes = await _mockRepo.Object.GetAll();
            leaveTypes.Count().ShouldBe(2);
        }
    }
}
