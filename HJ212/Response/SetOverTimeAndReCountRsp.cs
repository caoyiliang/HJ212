using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;
using Utils;

namespace HJ212.Response
{
    internal class SetOverTimeAndReCountRsp : IAsyncResponse<(int OverTime, int ReCount, RspInfo RspInfo)>
    {
        private int _overTime;
        private int _reCount;
        private readonly RspInfo _rspInfo = new();
        public async Task AnalyticalData(byte[] bytes)
        {
            var str = Encoding.ASCII.GetString(bytes.Skip(6).ToArray());
            var datalist = str.Split([";", ",", "&&"], StringSplitOptions.RemoveEmptyEntries).Where(item => item.Contains('=') && !item.Contains("CP"));
            _rspInfo.QN = datalist.FirstOrDefault(item => item.Contains("QN"));
            _rspInfo.ST = datalist.FirstOrDefault(item => item.Contains("ST"));
            _rspInfo.PW = datalist.FirstOrDefault(item => item.Contains("PW"));
            _rspInfo.MN = datalist.FirstOrDefault(item => item.Contains("MN"));
            if (!int.TryParse(datalist.SingleOrDefault(item => item.Contains("OverTime"))?.Split('=')[1], out _overTime))
            {
                throw new ArgumentException($"HJ212 Set OverTime Error");
            }
            if (!int.TryParse(datalist.SingleOrDefault(item => item.Contains("ReCount"))?.Split('=')[1], out _reCount))
            {
                throw new ArgumentException($"HJ212 Set ReCount Error");
            }
            await Task.CompletedTask;
        }

        public (bool Type, byte[]? CheckBytes) Check(byte[] bytes)
        {
            var rs = Encoding.ASCII.GetString(bytes).Split(';');
            return (rs.Where(item => item.Contains($"CN={(int)CN_Server.设置超时时间及重发次数}")).Any(), default);
        }

        public (int OverTime, int ReCount, RspInfo RspInfo) GetResult()
        {
            return (_overTime, _reCount, _rspInfo);
        }
    }
}
