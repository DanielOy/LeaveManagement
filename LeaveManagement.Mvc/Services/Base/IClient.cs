using System.Net.Http;

namespace LeaveManagement.Mvc.Services.Base
{
    public partial interface IClient
    {
        public HttpClient HttpClient { get; }
    }
}
