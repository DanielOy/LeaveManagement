using LeaveManagement.Mvc.Models;
using System.Threading.Tasks;

namespace LeaveManagement.Mvc.Contracts
{
    public interface IAuthenticationService
    {
        Task<bool> Login(string email, string password);
        Task<bool> Register(RegisterVM register);
        Task Logout();
    }
}
