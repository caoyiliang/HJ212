using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Response
{
    internal class SetNewSKRsp : IAsyncResponse<(string SKCreateTime, string NewSK, RspInfo RspInfo)>
    {
        private string _SKCreateTime = "";
        private string _NewSK = "";
        private readonly RspInfo _rspInfo = new();
        public async Task AnalyticalData(byte[] bytes)
        {
            var str = Encoding.ASCII.GetString(bytes.Skip(6).ToArray());
            var datalist = str.Split([";", ",", "&&"], StringSplitOptions.RemoveEmptyEntries).Where(item => item.Contains('=') && !item.Contains("CP"));
            _rspInfo.QN = datalist.FirstOrDefault(item => item.Contains("QN"));
            _rspInfo.ST = datalist.FirstOrDefault(item => item.Contains("ST"));
            _rspInfo.PW = datalist.FirstOrDefault(item => item.Contains("PW"));
            _rspInfo.MN = datalist.FirstOrDefault(item => item.Contains("MN"));
            _SKCreateTime = datalist.SingleOrDefault(item => item.Contains("SKCreateTime"))?.Split('=')[1] ?? "";
            _NewSK = datalist.SingleOrDefault(item => item.Contains("NewSK"))?.Split('=')[1] ?? "";
            await Task.CompletedTask;
        }

        public (bool Type, byte[]? CheckBytes) Check(byte[] bytes)
        {
            var rs = Encoding.ASCII.GetString(bytes).Split(';');
            return (rs.Where(item => item.Contains($"CN={(int)CN_Server.上位机下发新密钥}")).Any(), default);
        }

        public (string SKCreateTime, string NewSK, RspInfo RspInfo) GetResult()
        {
            return (_SKCreateTime, _NewSK, _rspInfo);
        }
    }
}
