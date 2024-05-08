using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;
using Utils;

namespace HJ212.Response
{
    internal class GetInfoRsp : IAsyncResponse<(string PolId, string InfoId, RspInfo RspInfo)>
    {
        private string _polId = null!;
        private string _infoId = null!;
        private RspInfo _rspInfo = new();
        public async Task AnalyticalData(byte[] bytes)
        {
            var str = Encoding.ASCII.GetString(bytes.Skip(6).ToArray());
            var datalist = str.Split([";", ",", "&&"], StringSplitOptions.RemoveEmptyEntries).Where(item => item.Contains('=') && !item.Contains("CP"));
            _rspInfo.QN = datalist.FirstOrDefault(item => item.Contains("QN"));
            _rspInfo.ST = datalist.FirstOrDefault(item => item.Contains("ST"));
            _rspInfo.PW = datalist.FirstOrDefault(item => item.Contains("PW"));
            _rspInfo.MN = datalist.FirstOrDefault(item => item.Contains("MN"));
            _polId = datalist.SingleOrDefault(item => item.Contains("PolId"))?.Split('=')[1] ?? throw new ArgumentException($"{GB._name} HJ212 Get Info Error");
            _infoId = datalist.SingleOrDefault(item => item.Contains("InfoId"))?.Split('=')[1] ?? throw new ArgumentException($"{GB._name} HJ212 Get Info Error");
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
            return (rs.Where(item => item.Contains("CN=3020")).Any() && rs.Where(item => !item.Contains("InfoId=i11001")).Any(), default);
        }

        public (string PolId, string InfoId, RspInfo RspInfo) GetResult()
        {
            return (_polId, _infoId, _rspInfo);
        }
    }
}
