﻿using AutoMapper;
using LeaveManagement.Application.Contracts.Persitence;
using LeaveManagement.Application.DTOs.LeaveAllocation;
using LeaveManagement.Application.Features.LeaveAllocations.Handlers.Queries;
using LeaveManagement.Application.Features.LeaveAllocations.Requests.Queries;
using LeaveManagement.Application.Profiles;
using LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;

namespace LeaveManagement.Application.UnitTests.LeaveAllocations.Queries
{
    public class GetLeaveAllocationDetailRequestHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveAllocationRepository> _mockRepo;

        public GetLeaveAllocationDetailRequestHandlerTests()
        {
            _mockRepo = MockLeaveAllocationRepository.GetLeaveAllocationRepository();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task GetLeaveAllocationDetailTest()
        {
            //Arrange
            var handler = new GetLeaveAllocationDetailRequestHandler(_mockRepo.Object, _mapper);
            var request = new GetLeaveAllocationDetailRequest() { Id = 1 };

            //Act
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.ShouldBeOfType<LeaveAllocationDto>();
            result.Id.ShouldBe(1);
        }
    }
}
