using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;
using Utils;

namespace HJ212.Response
{
    internal class SetSamplingPeriodRsp : IAsyncResponse<(string PolId, TimeOnly CstartTime, int Ctime, RspInfo RspInfo)>
    {
        private string _polId = null!;
        private int _Ctime;
        private TimeOnly _CstartTime;
        private RspInfo _rspInfo = new();
        public async Task AnalyticalData(byte[] bytes)
        {
            var str = Encoding.ASCII.GetString(bytes.Skip(6).ToArray());
            var datalist = str.Split([";", ",", "&&"], StringSplitOptions.RemoveEmptyEntries).Where(item => item.Contains('=') && !item.Contains("CP"));
            _rspInfo.QN = datalist.FirstOrDefault(item => item.Contains("QN"));
            _rspInfo.ST = datalist.FirstOrDefault(item => item.Contains("ST"));
            _rspInfo.PW = datalist.FirstOrDefault(item => item.Contains("PW"));
            _rspInfo.MN = datalist.FirstOrDefault(item => item.Contains("MN"));
            _polId = datalist.SingleOrDefault(item => item.Contains("PolId"))?.Split('=')[1] ?? throw new ArgumentException($"HJ212 Set SamplingPeriod Error");
            if (!int.TryParse(datalist.SingleOrDefault(item => item.Contains("CTime"))?.Split('=')[1], out _Ctime))
            {
                throw new ArgumentException($"HJ212 Set SamplingPeriod CTime Error");
            }
            if (!TimeOnly.TryParseExact(datalist.SingleOrDefault(item => item.Contains("CstartTime"))?.Split('=')[1], "HHmmss", null, System.Globalization.DateTimeStyles.None, out _CstartTime))
            {
                throw new ArgumentException($"HJ212 Set SamplingPeriod CstartTime Error");
            }
            await Task.CompletedTask;
        }

        public (bool Type, byte[]? CheckBytes) Check(byte[] bytes)
        {
            var rs = Encoding.ASCII.GetString(bytes).Split(';');
            return (rs.Where(item => item.Contains($"CN={(int)CN_Server.设置采样时间周期}")).Any(), default);
        }

        public (string PolId, TimeOnly CstartTime, int Ctime, RspInfo RspInfo) GetResult()
        {
            return (_polId, _CstartTime, _Ctime, _rspInfo);
        }
    }
}
