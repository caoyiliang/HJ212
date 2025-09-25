using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadStatisticsDataReq : IAsyncRequest
    {
        private readonly string _QN;
        private readonly string _rs;

        public UploadStatisticsDataReq(CN_Client cn, string? mn, string pw, ST st, DateTime dataTime, List<StatisticsData> data, int pnum, int pno, Version version, Func<string, string> func, DateTime? sendTime = null)
        {
            _QN = sendTime == null ? DateTime.Now.ToString("yyyyMMddHHmmssfff") : sendTime.Value.ToString("yyyyMMddHHmmssfff");
            _rs = func.Invoke($"QN={_QN};ST={(int)st};CN={(int)cn};PW={pw};MN={mn};Flag={(pnum > 1 ? $"{3 | (int)version};PNUM={pnum};PNO={pno}" : $"{1 | (int)version}")};CP=&&DataTime={dataTime:yyyyMMddHHmmss};{string.Join(";", data.Select(c => $"{(c.Cou != null ? $"{c.Name}-Cou={c.Cou}," : "")}{(c.Min != null ? $"{c.Name}-Min={c.Min}," : "")}{c.Name}-Avg={c.Avg}{(c.Max != null ? $",{c.Name}-Max={c.Max}" : "")}{(c.Flag != null ? $",{c.Name}-Flag={c.Flag}" : "")}{(c.Other.Count > 0 ? $",{string.Join(",", c.Other.Select(i => $"{c.Name}-{i.Key}={i.Value}"))}" : "")}"))}&&");
        }

        public UploadStatisticsDataReq(string data, string name)
        {
            _rs = data;
            var datalist = data.Split([";", ",", "&&"], StringSplitOptions.RemoveEmptyEntries).Where(item => item.Contains('=') && !item.Contains("CP"));
            _QN = datalist.SingleOrDefault(item => item.Contains("QN"))?.Split('=')[1] ?? throw new ArgumentException($"{name} HJ212 ReissueStatistics QN Error");
        }

        public byte[]? Check()
        {
            return Encoding.ASCII.GetBytes(_QN);
        }

        public byte[] ToBytes()
        {
            return Encoding.ASCII.GetBytes(_rs);
        }

        public override string ToString()
        {
            return _rs;
        }
    }
}
