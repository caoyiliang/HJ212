using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Response
{
    internal class CalibrateRsp : IAsyncResponse<(string PolId, CalibrationType? CalibrationType, RspInfo RspInfo)>
    {
        private string _polId = null!;
        private CalibrationType? _calibrationType;
        private RspInfo _rspInfo = new();
        public async Task AnalyticalData(byte[] bytes)
        {
            var str = Encoding.ASCII.GetString(bytes.Skip(6).ToArray());
            var datalist = str.Split([";", ",", "&&"], StringSplitOptions.RemoveEmptyEntries).Where(item => item.Contains('=') && !item.Contains("CP"));
            _rspInfo.QN = datalist.FirstOrDefault(item => item.Contains("QN"));
            _rspInfo.ST = datalist.FirstOrDefault(item => item.Contains("ST"));
            _rspInfo.PW = datalist.FirstOrDefault(item => item.Contains("PW"));
            _rspInfo.MN = datalist.FirstOrDefault(item => item.Contains("MN"));
            _polId = datalist.SingleOrDefault(item => item.Contains("PolId"))?.Split('=')[1] ?? throw new ArgumentException($"HJ212 Calibrate PolId Error");
            var calibrationTypeStr = datalist.SingleOrDefault(item => item.Contains("CalibrationType"))?.Split('=')[1];
            if (Enum.TryParse(calibrationTypeStr, out CalibrationType calibrationType))
            {
                _calibrationType = calibrationType;
            }
            await Task.CompletedTask;
        }

        public (bool Type, byte[]? CheckBytes) Check(byte[] bytes)
        {
            var rs = Encoding.ASCII.GetString(bytes).Split(';');
            return (rs.Where(item => item.Contains($"CN={(int)CN_Server.零点校准量程校准}")).Any(), default);
        }

        public (string PolId, CalibrationType? CalibrationType, RspInfo RspInfo) GetResult()
        {
            return (_polId, _calibrationType, _rspInfo);
        }
    }
}
