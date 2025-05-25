namespace Redis_Example_Session.Models.Dtos
{
    public class SessionData
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;
        public DateTime LoginTime { get; set; }
    }
}
