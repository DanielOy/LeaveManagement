using AutoMapper;
using LeaveManagement.Application.Contracts.Persitence;
using LeaveManagement.Application.DTOs.LeaveType;
using LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands;
using LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using LeaveManagement.Application.Profiles;
using LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LeaveManagement.Application.UnitTests.LeaveTypes.Commands
{
    public class UpdateLeaveTypeCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public UpdateLeaveTypeCommandHandlerTests()
        {
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task Valid_LeaveType_Updated()
        {
            //Arrange
            var handler = new UpdateLeaveTypeCommandHandler(_mapper, _mockUnitOfWork.Object);
            var request = new UpdateLeaveTypeCommand();
            var leaveTypeDto = new LeaveTypeDto
            {
                Id = 1,
                DefaultDays = 1,
                Name = "Test"
            };
            request.LeaveTypeDto = leaveTypeDto;


            //Act
            await handler.Handle(request, CancellationToken.None);

            var leaveType = await _mockUnitOfWork.Object.LeaveTypeRepository.Get(1);

            //Assert
            leaveType.Name.ShouldBe(leaveTypeDto.Name);
            leaveType.DefaultDays.ShouldBe(leaveTypeDto.DefaultDays);
        }

        [Test]
        public async Task Invalid_LeaveType_Updated()
        {
            //Arrange
            var handler = new UpdateLeaveTypeCommandHandler(_mapper, _mockUnitOfWork.Object);
            var request = new UpdateLeaveTypeCommand();
            request.LeaveTypeDto = new LeaveTypeDto
            {
                Id = 3,
                DefaultDays = -1,
                Name = "Test Dto"
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
