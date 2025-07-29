using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Response
{
    internal class GetLogInfosRsp : IAsyncResponse<(string? PolId, DateTime BeginTime, DateTime EndTime, RspInfo RspInfo)>
    {
        private string? _polId;
        private DateTime _beginTime;
        private DateTime _endTime;
        private readonly RspInfo _rspInfo = new();
        public async Task AnalyticalData(byte[] bytes)
        {
            var str = Encoding.ASCII.GetString(bytes.Skip(6).ToArray());
            var datalist = str.Split([";", ",", "&&"], StringSplitOptions.RemoveEmptyEntries).Where(item => item.Contains('=') && !item.Contains("CP"));
            _rspInfo.QN = datalist.FirstOrDefault(item => item.Contains("QN"));
            _rspInfo.ST = datalist.FirstOrDefault(item => item.Contains("ST"));
            _rspInfo.PW = datalist.FirstOrDefault(item => item.Contains("PW"));
            _rspInfo.MN = datalist.FirstOrDefault(item => item.Contains("MN"));
            _polId = datalist.SingleOrDefault(item => item.Contains("PolId"))?.Split('=')[1];
            if (!DateTime.TryParseExact(datalist.SingleOrDefault(item => item.Contains("BeginTime"))?.Split('=')[1], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out _beginTime))
            {
                throw new ArgumentException($"HJ212 Get LogInfos BeginTime Error");
            }
            if (!DateTime.TryParseExact(datalist.SingleOrDefault(item => item.Contains("EndTime"))?.Split('=')[1], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out _endTime))
            {
                throw new ArgumentException($"HJ212 Get LogInfos EndTime Error");
            }
            await Task.CompletedTask;
        }

        public (bool Type, byte[]? CheckBytes) Check(byte[] bytes)
        {
            var rs = Encoding.ASCII.GetString(bytes).Split(';');
            return (rs.Where(item => item.Contains($"CN={(int)CN_Server.提取现场机信息}")).Any() && rs.Where(item => item.Contains("InfoId=i11001")).Any(), default);
        }

        public (string? PolId, DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) GetResult()
        {
            return (_polId, _beginTime, _endTime, _rspInfo);
        }
    }
}
