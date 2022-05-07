namespace LeaveManagement.Application.Responses
{
    public class BaseCommandResponse
    {
        public int Id { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
