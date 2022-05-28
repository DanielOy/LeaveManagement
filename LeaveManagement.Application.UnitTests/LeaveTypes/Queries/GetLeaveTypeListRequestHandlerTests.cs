using AutoMapper;
using LeaveManagement.Application.Contracts.Persitence;
using LeaveManagement.Application.DTOs.LeaveType;
using LeaveManagement.Application.Features.LeaveTypes.Handlers.Queries;
using LeaveManagement.Application.Features.LeaveTypes.Requests.Queries;
using LeaveManagement.Application.Profiles;
using LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeaveManagement.Application.UnitTests.LeaveTypes.Queries
{
    public class GetLeaveTypeListRequestHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public GetLeaveTypeListRequestHandlerTests()
        {
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task GetLeaveTypeListTest()
        {
            //Arrange
            var handler = new GetLeaveTypeListRequestHandler(_mapper, _mockUnitOfWork.Object);
            var request = new GetLeaveTypeListRequest();

            //Act
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.ShouldBeOfType<List<LeaveTypeDto>>();
            result.Count().ShouldBe(2);
        }
    }
}
