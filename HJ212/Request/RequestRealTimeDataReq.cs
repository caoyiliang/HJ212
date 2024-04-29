using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class RequestRealTimeDataReq : IAsyncRequest
    {
        private readonly string _mn;
        private readonly string _pw;
        private readonly ST _st;
        private readonly DateTime _dataTime;
        private readonly List<RealTimeData> _data;
        private readonly string _QN;

        public RequestRealTimeDataReq(string mn, string pw, ST st, DateTime dataTime, List<RealTimeData> data)
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
            var rs = $"QN={_QN};ST={(int)_st};CN={(int)CN.实时数据};PW={_pw};MN={_mn};CP=&&DataTime={_dataTime:yyyyMMddHHmmss};{string.Join(";", _data.Select(c => $"{(c.SampleTime != null ? $",{c.Name}-SampleTime={c.SampleTime}," : "")}{c.Name}-Rtd={c.Rtd}{(c.Flag != null ? $",{c.Name}-Flag={c.Flag}" : "")}{(c.EFlag != null ? $",{c.Name}-EFlag={c.EFlag}" : "")}"))}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
