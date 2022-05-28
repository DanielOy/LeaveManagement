using AutoMapper;
using LeaveManagement.Application.Contracts.Identity;
using LeaveManagement.Application.Contracts.Persitence;
using LeaveManagement.Application.DTOs.LeaveAllocation;
using LeaveManagement.Application.Exceptions;
using LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands;
using LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using LeaveManagement.Application.Profiles;
using LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeaveManagement.Application.UnitTests.LeaveAllocation.Commands
{
    public class CreateLeaveAllocationCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserService> _mockUserService;

        public CreateLeaveAllocationCommandHandlerTests()
        {
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
            _mockUserService = MockUserService.GetUserService();

            var mapperConfig = new MapperConfiguration(c =>
           {
               c.AddProfile<MappingProfile>();
           });

            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task Valid_LeaveAllocation_Added()
        {
            //Arrange
            var handler = new CreateLeaveAllocationCommandHandler(_mapper, _mockUserService.Object, _mockUnitOfWork.Object);
            var request = new CreateLeaveAllocationCommand();
            request.CreateLeaveAllocationDto = new CreateLeaveAllocationDto
            {
                LeaveTypeId = 1
            };


            //Act
            var result = await handler.Handle(request, CancellationToken.None);

            var LeaveAllocations = await _mockUnitOfWork.Object.LeaveAllocationRepository.GetAll();

            //Assert
            result.ShouldBeOfType<int>();
            LeaveAllocations.Count().ShouldBe(3);
        }

        [Test]
        public async Task Invalid_LeaveAllocation_Added()
        {
            //Arrange
            var handler = new CreateLeaveAllocationCommandHandler(_mapper, _mockUserService.Object, _mockUnitOfWork.Object);
            var request = new CreateLeaveAllocationCommand();
            request.CreateLeaveAllocationDto = new CreateLeaveAllocationDto
            {
                LeaveTypeId = 3
            };

            //Act
            var exception = await Should.ThrowAsync<ValidationException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });


            //Assert
            exception.ShouldNotBeNull();

            var LeaveAllocations = await _mockUnitOfWork.Object.LeaveAllocationRepository.GetAll();
            LeaveAllocations.Count().ShouldBe(2);
        }
    }
}
