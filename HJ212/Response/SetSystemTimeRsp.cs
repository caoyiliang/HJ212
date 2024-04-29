using System.Text;
using HJ212.Model;
using TopPortLib.Interfaces;
using Utils;

namespace HJ212.Response
{
    internal class SetSystemTimeRsp : IAsyncResponse<(string? PolId, DateTime SystemTime, RspInfo RspInfo)>
    {
        private string? _polId;
        private DateTime _systemTime;
        private RspInfo _rspInfo = new();
        public async Task AnalyticalData(byte[] bytes)
        {
            var str = Encoding.ASCII.GetString(bytes.Skip(6).ToArray());
            var datalist = str.Split([";", ",", "&&"], StringSplitOptions.RemoveEmptyEntries).Where(item => item.Contains('=') && !item.Contains("CP"));
            _rspInfo.QN = datalist.FirstOrDefault(item => item.Contains("QN"));
            _rspInfo.ST = datalist.FirstOrDefault(item => item.Contains("ST"));
            _rspInfo.PW = datalist.FirstOrDefault(item => item.Contains("PW"));
            _rspInfo.MN = datalist.FirstOrDefault(item => item.Contains("MN"));
            _polId = datalist.SingleOrDefault(item => item.Contains("PolId"))?.Split('=')[1];
            if (DateTime.TryParseExact(datalist.SingleOrDefault(item => item.Contains("SystemTime"))?.Split('=')[1], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out _systemTime))
            {
                throw new ArgumentException($"{GB._name} HJ212 Set SystemTime Error");
            }
            await Task.CompletedTask;
        }

        public (bool Type, byte[]? CheckBytes) Check(byte[] bytes)
        {
            var data = bytes.Skip(6).ToArray();
            var dstr = Encoding.ASCII.GetString(data);
            if (StringByteUtils.BytesToString(CRC.GBcrc16(data, data.Length - 4)).Replace(" ", "") != dstr[^4..])
            {
                throw new ArgumentException($"{GB._name} HJ212 CRC Error: {dstr}", nameof(bytes));
            }
            var rs = dstr.Split(';');
            return (rs.Where(item => item.Contains("CN=1012")).Any(), default);
        }

        public (string? PolId, DateTime SystemTime, RspInfo RspInfo) GetResult()
        {
            return (_polId, _systemTime, _rspInfo);
        }
    }
}
