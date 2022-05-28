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
using System.Threading;
using System.Threading.Tasks;

namespace LeaveManagement.Application.UnitTests.LeaveTypes.Queries
{
    public class GetLeaveTypeDetailRequestHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public GetLeaveTypeDetailRequestHandlerTests()
        {
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task GetLeaveTypeDetailTest()
        {
            //Arrange
            var handler = new GetLeaveTypeDetailRequestHandler(_mapper, _mockUnitOfWork.Object);
            var request = new GetLeaveTypeDetailRequest() { Id = 1 };

            //Act
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.ShouldBeOfType<LeaveTypeDto>();
            result.Id.ShouldBe(1);
        }
    }
}
