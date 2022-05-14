using LeaveManagement.Application.Contracts.Infrastructure;
using LeaveManagement.Application.Models;
using Moq;

namespace LeaveManagement.Application.UnitTests.Mocks
{
    public static class MockEmailSender
    {
        public static Mock<IEmailSender> GetEmailSender()
        {
            var emailSender = new Mock<IEmailSender>();

            emailSender.Setup(x => x.SendEmail(It.IsAny<Email>()))
                .ReturnsAsync(() =>
                {
                    return true;
                });

            return emailSender;
        }
    }
}
