namespace HJ212.Model
{
    public class LogInfo(string info, DateTime? dataTime = default)
    {
        public string? PolId { get; set; }
        public DateTime DataTime { get; set; } = dataTime ?? DateTime.Now;
        public string Info { get; set; } = info;
    }
}
