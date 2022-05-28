using AutoMapper;
using LeaveManagement.Application.Contracts.Persitence;
using LeaveManagement.Application.DTOs.LeaveAllocation;
using LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands;
using LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using LeaveManagement.Application.Profiles;
using LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LeaveManagement.Application.UnitTests.LeaveAllocation.Commands
{
    public class UpdateLeaveAllocationCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public UpdateLeaveAllocationCommandHandlerTests()
        {
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task Valid_LeaveAllocation_Updated()
        {
            //Arrange
            var handler = new UpdateLeaveAllocationCommandHandler(_mapper, _mockUnitOfWork.Object);
            var request = new UpdateLeaveAllocationCommand();
            var LeaveAllocationDto = new UpdateLeaveAllocationDto
            {
                Id = 1,
                Period = 2023,
                NumberOfDays = 2,
                LeaveTypeId = 1
            };
            request.UpdateLeaveAllocationDto = LeaveAllocationDto;


            //Act
            await handler.Handle(request, CancellationToken.None);

            var LeaveAllocation = await _mockUnitOfWork.Object.LeaveAllocationRepository.Get(LeaveAllocationDto.Id);

            //Assert
            LeaveAllocation.Period.ShouldBe(LeaveAllocationDto.Period);
            LeaveAllocation.NumberOfDays.ShouldBe(LeaveAllocationDto.NumberOfDays);
            LeaveAllocation.LeaveTypeId.ShouldBe(LeaveAllocationDto.LeaveTypeId);
        }

        [Test]
        public async Task Invalid_LeaveAllocation_Updated()
        {
            //Arrange
            var handler = new UpdateLeaveAllocationCommandHandler(_mapper, _mockUnitOfWork.Object);
            var request = new UpdateLeaveAllocationCommand();
            request.UpdateLeaveAllocationDto = new UpdateLeaveAllocationDto
            {
                Id = 3,
                Period = 2,
                NumberOfDays = 2,
                LeaveTypeId = 2
            };

            //Act
            var exception = await Should.ThrowAsync<ApplicationException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });


            //Assert
            exception.ShouldNotBeNull();
        }
    }
}
