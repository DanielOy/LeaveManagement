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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeaveManagement.Application.UnitTests.LeaveTypes.Commands
{
    public class CreateLeaveTypeCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveTypeRepository> _mockRepo;

        public CreateLeaveTypeCommandHandlerTests()
        {
            _mockRepo = MockLeaveTypeRepository.GetLeaveTypeRepository();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task Valid_LeaveType_Added()
        {
            //Arrange
            var handler = new CreateLeaveTypeCommandHandler(_mockRepo.Object, _mapper);
            var request = new CreateLeaveTypeCommand();
            request.CreateLeaveTypeDto = new CreateLeaveTypeDto
            {
                DefaultDays = 15,
                Name = "Test Dto"
            };


            //Act
            var result = await handler.Handle(request, CancellationToken.None);

            var leaveTypes = await _mockRepo.Object.GetAll();

            //Assert
            result.ShouldBeOfType<int>();
            leaveTypes.Count().ShouldBe(3);
        }

        [Test]
        public async Task Invalid_LeaveType_Added()
        {
            //Arrange
            var handler = new CreateLeaveTypeCommandHandler(_mockRepo.Object, _mapper);
            var request = new CreateLeaveTypeCommand();
            request.CreateLeaveTypeDto = new CreateLeaveTypeDto
            {
                DefaultDays = -1,
                Name = "Test Dto"
            };

            //Act
            var exception = await Should.ThrowAsync<ValidationException>(async () =>
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
