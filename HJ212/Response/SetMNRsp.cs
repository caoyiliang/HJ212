using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Response
{
    internal class SetMNRsp : IAsyncResponse<(string? DataLoggerId, RspInfo RspInfo)>
    {
        private string? _dataLoggerId;
        private readonly RspInfo _rspInfo = new();
        public async Task AnalyticalData(byte[] bytes)
        {
            var str = Encoding.ASCII.GetString(bytes.Skip(6).ToArray());
            var datalist = str.Split([";", ",", "&&"], StringSplitOptions.RemoveEmptyEntries).Where(item => item.Contains('=') && !item.Contains("CP"));
            _rspInfo.QN = datalist.FirstOrDefault(item => item.Contains("QN"));
            _rspInfo.ST = datalist.FirstOrDefault(item => item.Contains("ST"));
            _rspInfo.PW = datalist.FirstOrDefault(item => item.Contains("PW"));
            _rspInfo.MN = datalist.FirstOrDefault(item => item.Contains("MN"));
            _dataLoggerId = datalist.SingleOrDefault(item => item.Contains("i23005-Info"))?.Split('=')[1];
            await Task.CompletedTask;
        }

        public (bool Type, byte[]? CheckBytes) Check(byte[] bytes)
        {
            var rs = Encoding.ASCII.GetString(bytes).Split(';');
            return (rs.Where(item => item.Contains($"CN={(int)CN_Server.设置数采仪MN编码}")).Any(), default);
        }

        public (string? DataLoggerId, RspInfo RspInfo) GetResult()
        {
            return (_dataLoggerId, _rspInfo);
        }
    }
}
