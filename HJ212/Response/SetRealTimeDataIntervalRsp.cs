using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;
using Utils;

namespace HJ212.Response
{
    internal class SetRealTimeDataIntervalRsp : IAsyncResponse<(int RtdInterval, RspInfo RspInfo)>
    {
        private int _rtdInterval;
        private RspInfo _rspInfo = new();
        public async Task AnalyticalData(byte[] bytes)
        {
            var str = Encoding.ASCII.GetString(bytes.Skip(6).ToArray());
            var datalist = str.Split([";", ",", "&&"], StringSplitOptions.RemoveEmptyEntries).Where(item => item.Contains('=') && !item.Contains("CP"));
            _rspInfo.QN = datalist.FirstOrDefault(item => item.Contains("QN"));
            _rspInfo.ST = datalist.FirstOrDefault(item => item.Contains("ST"));
            _rspInfo.PW = datalist.FirstOrDefault(item => item.Contains("PW"));
            _rspInfo.MN = datalist.FirstOrDefault(item => item.Contains("MN"));
            if (!int.TryParse(datalist.SingleOrDefault(item => item.Contains("RtdInterval"))?.Split('=')[1], out _rtdInterval))
            {
                throw new ArgumentException($"{GB._name} HJ212 Set RtdInterval Error");
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
            return (rs.Where(item => item.Contains($"CN={(int)CN_Server.设置实时数据间隔}")).Any(), default);
        }

        public (int RtdInterval, RspInfo RspInfo) GetResult()
        {
            return (_rtdInterval, _rspInfo);
        }
    }
}
