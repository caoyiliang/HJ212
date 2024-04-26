using System.Text;
using TopPortLib.Interfaces;
using Utils;

namespace HJ212.Response
{
    internal class GetSystemTimeRsp : IAsyncResponse<(string? PolId, RspInfo RspInfo)>
    {
        private string? _polId;
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
            await Task.CompletedTask;
        }

        public bool Check(byte[] bytes)
        {
            var data = bytes.Skip(6).ToArray();
            var dstr = Encoding.ASCII.GetString(data);
            if (StringByteUtils.BytesToString(CRC.GBcrc16(data, data.Length - 4)).Replace(" ", "") != dstr[^4..])
            {
                throw new ArgumentException($"CRC Error: {dstr}", nameof(bytes));
            }
            var rs = dstr.Split(';');
            return rs.Where(item => item.Contains("CN=1011")).Any();
        }

        public (string? PolId, RspInfo RspInfo) GetResult()
        {
            return (_polId, _rspInfo);
        }
    }
}
