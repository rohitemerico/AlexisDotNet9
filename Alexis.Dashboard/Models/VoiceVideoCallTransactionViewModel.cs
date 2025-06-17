namespace Alexis.Dashboard.Models
{
    public class VoiceVideoCallTransactionViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UName { get; set; }
        public string CallType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int DurationSeconds { get; set; }
        public string Status { get; set; }
    }
}