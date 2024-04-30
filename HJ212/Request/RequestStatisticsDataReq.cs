using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class RequestStatisticsDataReq : IAsyncRequest
    {
        private readonly CN _cn;
        private readonly string _mn;
        private readonly string _pw;
        private readonly ST _st;
        private readonly DateTime _dataTime;
        private readonly List<StatisticsData> _data;
        private readonly string _QN;

        public RequestStatisticsDataReq(CN cn, string mn, string pw, ST st, DateTime dataTime, List<StatisticsData> data)
        {
            _cn = cn;
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
            var rs = $"QN={_QN};ST={(int)_st};CN={(int)_cn};PW={_pw};MN={_mn};CP=&&DataTime={_dataTime:yyyyMMddHHmm00};{string.Join(";", _data.Select(c => $"{(c.Cou != null ? $"{c.Name}-Cou={c.Cou}," : "")}{c.Name}-Min={c.Min},{c.Name}-Avg={c.Avg},{c.Name}-Max={c.Max}{(c.Flag != null ? $",{c.Name}-Flag={c.Flag}" : "")}"))}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
