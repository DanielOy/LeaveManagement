using AutoMapper;
using LeaveManagement.Application.Contracts.Infrastructure;
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
    public class CreateLeaveRequestCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveRequestRepository> _mockRepo;
        private readonly Mock<IEmailSender> _mockEmail;

        public CreateLeaveRequestCommandHandlerTests()
        {
            _mockRepo = MockLeaveRequestRepository.GetLeaveRequestRepository();
            _mockEmail = MockEmailSender.GetEmailSender();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task Valid_LeaveRequest_Added()
        {
            //Arrange
            var handler = new CreateLeaveRequestCommandHandler(_mockRepo.Object, _mapper, _mockEmail.Object);
            var request = new CreateLeaveRequestCommand();
            request.CreateLeaveRequestDto = new CreateLeaveRequestDto
            {
                LeaveTypeId = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                RequestComments = "Comment"
            };


            //Act
            var result = await handler.Handle(request, CancellationToken.None);

            var LeaveRequests = await _mockRepo.Object.GetAll();

            //Assert
            result.ShouldNotBeNull();
            LeaveRequests.Count().ShouldBe(3);
        }

        [Test]
        public async Task Invalid_LeaveRequest_Added()
        {
            //Arrange
            var handler = new CreateLeaveRequestCommandHandler(_mockRepo.Object, _mapper, _mockEmail.Object);
            var request = new CreateLeaveRequestCommand();
            request.CreateLeaveRequestDto = new CreateLeaveRequestDto
            {
                LeaveTypeId = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(-2),
                RequestComments = "Comment"
            };

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            response.ShouldNotBeNull();
            response.Success.ShouldBeFalse();   

            var LeaveRequests = await _mockRepo.Object.GetAll();
            LeaveRequests.Count().ShouldBe(2);
        }
    }
}
