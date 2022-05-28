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
using System.Threading;
using System.Threading.Tasks;

namespace LeaveManagement.Application.UnitTests.LeaveRequests.Commands
{
    public class UpdateLeaveRequestCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public UpdateLeaveRequestCommandHandlerTests()
        {
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task Valid_LeaveRequest_Updated()
        {
            //Arrange
            var handler = new UpdateLeaveRequestCommandHandler(_mapper, _mockUnitOfWork.Object);
            var request = new UpdateLeaveRequestCommand();
            var LeaveRequestDto = new UpdateLeaveRequestDto
            {
                Id = 1,
                StartDate = DateTime.Now.AddDays(3),
                EndDate = DateTime.Now.AddDays(5),
                LeaveTypeId = 1,
                Cancelled = false,
                RequestComments = "Test Comment"
            };
            request.LeaveRequestDto = LeaveRequestDto;


            //Act
            await handler.Handle(request, CancellationToken.None);

            var LeaveRequest = await _mockUnitOfWork.Object.LeaveRequestRepository.Get(1);

            //Assert
            LeaveRequest.StartDate.ShouldBe(LeaveRequestDto.StartDate);
            LeaveRequest.EndDate.ShouldBe(LeaveRequestDto.EndDate);
            LeaveRequest.LeaveTypeId.ShouldBe(LeaveRequestDto.LeaveTypeId);
            LeaveRequest.Cancelled.ShouldBe(LeaveRequestDto.Cancelled);
            LeaveRequest.RequestComments.ShouldBe(LeaveRequestDto.RequestComments);
        }

        [Test]
        public async Task Invalid_LeaveRequest_Updated()
        {
            //Arrange
            var handler = new UpdateLeaveRequestCommandHandler(_mapper, _mockUnitOfWork.Object);
            var request = new UpdateLeaveRequestCommand();
            request.LeaveRequestDto = new UpdateLeaveRequestDto
            {
                Id = 3,
                StartDate = DateTime.Now.AddDays(6),
                EndDate = DateTime.Now.AddDays(5),
                LeaveTypeId = 3,
                Cancelled = false,
                RequestComments = "Test Comment"
            };

            //Act
            var exception = await Should.ThrowAsync<ValidationException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });


            //Assert
            exception.ShouldNotBeNull();
        }
    }
}
