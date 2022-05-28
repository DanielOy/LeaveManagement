using AutoMapper;
using LeaveManagement.Application.Contracts.Identity;
using LeaveManagement.Application.Contracts.Persitence;
using LeaveManagement.Application.DTOs.LeaveRequest;
using LeaveManagement.Application.Features.LeaveRequests.Handlers.Queries;
using LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using LeaveManagement.Application.Profiles;
using LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeaveManagement.Application.UnitTests.LeaveRequests.Queries
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
        public async Task GetLeaveRequestListTest()
        {
            //Arrange
            var handler = new GetLeaveRequestListRequestHandler(_mapper, null, _mockUserService.Object, _mockUnitOfWork.Object);
            var request = new GetLeaveRequestListRequest();

            //Act
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.ShouldBeOfType<List<LeaveRequestListDto>>();
            result.Count.ShouldBe(2);
        }
    }
}
