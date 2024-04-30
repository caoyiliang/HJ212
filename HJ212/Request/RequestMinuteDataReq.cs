using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class RequestMinuteDataReq : IAsyncRequest
    {
        private readonly string _mn;
        private readonly string _pw;
        private readonly ST _st;
        private readonly DateTime _dataTime;
        private readonly List<MinuteData> _data;
        private readonly string _QN;

        public RequestMinuteDataReq(string mn, string pw, ST st, DateTime dataTime, List<MinuteData> data)
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
            var rs = $"QN={_QN};ST={(int)_st};CN={(int)CN.分钟数据};PW={_pw};MN={_mn};CP=&&DataTime={_dataTime:yyyyMMddHHmm00};{string.Join(";", _data.Select(c => $"{(c.Cou != null ? $"{c.Name}-Cou={c.Cou}," : "")}{c.Name}-Min={c.Min},{c.Name}-Avg={c.Avg},{c.Name}-Max={c.Max}{(c.Flag != null ? $",{c.Name}-Flag={c.Flag}" : "")}"))}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
