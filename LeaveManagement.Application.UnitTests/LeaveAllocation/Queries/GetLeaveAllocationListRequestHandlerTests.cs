using AutoMapper;
using LeaveManagement.Application.Contracts.Identity;
using LeaveManagement.Application.Contracts.Persitence;
using LeaveManagement.Application.DTOs.LeaveAllocation;
using LeaveManagement.Application.Features.LeaveAllocations.Handlers.Queries;
using LeaveManagement.Application.Features.LeaveAllocations.Requests.Queries;
using LeaveManagement.Application.Profiles;
using LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeaveManagement.Application.UnitTests.LeaveAllocations.Queries
{
    public class GetLeaveAllocationListRequestHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserService> _mockUserService;

        public GetLeaveAllocationListRequestHandlerTests()
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
        public async Task GetLeaveAllocationListTest()
        {
            //Arrange
            var handler = new GetLeaveAllocationListRequestHandler(_mapper, null, _mockUserService.Object, _mockUnitOfWork.Object);
            var request = new GetLeaveAllocationListRequest();

            //Act
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.ShouldBeOfType<List<LeaveAllocationDto>>();
            result.Count().ShouldBe(2);
        }
    }
}
