using AutoMapper;
using LeaveManagement.Application.Contracts.Persitence;
using LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands;
using LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using LeaveManagement.Application.Profiles;
using LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeaveManagement.Application.UnitTests.LeaveAllocation.Commands
{
    public class DeleteLeaveAllocationCommandHandlerTests
    {
        private readonly Mock<ILeaveAllocationRepository> _mockRepoAllocation;
        private readonly Mock<ILeaveTypeRepository> _mockRepoType;

        public DeleteLeaveAllocationCommandHandlerTests()
        {
            _mockRepoAllocation = MockLeaveAllocationRepository.GetLeaveAllocationRepository();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
        }

        [Test]
        public async Task Valid_LeaveAllocation_Deleted()
        {
            //Arrange
            var handler = new DeleteLeaveAllocationCommandHandler(_mockRepoAllocation.Object);
            var request = new DeleteLeaveAllocationCommand() { Id = 1 };

            //Act
            var result = await handler.Handle(request, CancellationToken.None);

            var LeaveAllocations = await _mockRepoAllocation.Object.GetAll();

            //Assert
            LeaveAllocations.Count().ShouldBe(1);
        }

        [Test]
        public async Task Invalid_LeaveAllocation_Deleted()
        {
            //Arrange
            var handler = new DeleteLeaveAllocationCommandHandler(_mockRepoAllocation.Object);
            var request = new DeleteLeaveAllocationCommand() { Id = 3 };

            //Act
            var exception = await Should.ThrowAsync<ApplicationException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });


            //Assert
            exception.ShouldNotBeNull();

            var LeaveAllocations = await _mockRepoAllocation.Object.GetAll();
            LeaveAllocations.Count().ShouldBe(2);
        }
    }
}
