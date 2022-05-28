using LeaveManagement.Application.Contracts.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.UnitTests.Mocks
{
    public static class MockUserService
    {
        public static Mock<IUserService> GetUserService()
        {
            var userService = new Mock<IUserService>();

            
            return userService;
        }
    }
}
