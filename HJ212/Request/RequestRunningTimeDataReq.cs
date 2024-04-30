using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class RequestRunningTimeDataReq : IAsyncRequest
    {
        private readonly string _mn;
        private readonly string _pw;
        private readonly ST _st;
        private readonly DateTime _dataTime;
        private readonly List<RunningTimeData> _data;
        private readonly string _QN;

        public RequestRunningTimeDataReq(string mn, string pw, ST st, DateTime dataTime, List<RunningTimeData> data)
        {
            _mn = mn;
            _pw = pw;
            _st = st;
            _dataTime = dataTime;
            _data = data;
            _QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        public byte[]? Check()
        {
            return Encoding.ASCII.GetBytes(_QN);
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={_QN};ST={(int)_st};CN=2021;PW={_pw};MN={_mn};CP=&&DataTime={_dataTime:yyyyMMddHHmmss};{string.Join(";", _data.Select(c => $"{c.Name}-RT={c.RT}"))}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
