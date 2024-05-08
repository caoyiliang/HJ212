namespace HJ212.Model
{
    public class DeviceInfo(string infoId, string info)
    {
        public string InfoId { get; set; } = infoId;
        public string Info { get; set; } = info;
    }
}
