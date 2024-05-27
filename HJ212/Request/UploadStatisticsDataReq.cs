using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadStatisticsDataReq : IAsyncRequest
    {
        private readonly string _QN;
        private readonly string _rs;

        public UploadStatisticsDataReq(CN_Client cn, string mn, string pw, ST st, DateTime dataTime, List<StatisticsData> data, int pnum, int pno)
        {
            _QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            _rs = GB.GetGbCmd($"QN={_QN};ST={(int)st};CN={(int)cn};PW={pw};MN={mn};Flag={(pnum > 1 ? $"{3 | (int)GB._version};PNUM={pnum};PNO={pno}" : $"{1 | (int)GB._version}")};CP=&&DataTime={dataTime:yyyyMMddHHmmss};{string.Join(";", data.Select(c => $"{(c.Cou != null ? $"{c.Name}-Cou={c.Cou}," : "")}{c.Name}-Min={c.Min},{c.Name}-Avg={c.Avg},{c.Name}-Max={c.Max}{(c.Flag != null ? $",{c.Name}-Flag={c.Flag}" : "")}"))}&&");
        }

        public UploadStatisticsDataReq(string data)
        {
            _rs = data;
            var datalist = data.Split([";", ",", "&&"], StringSplitOptions.RemoveEmptyEntries).Where(item => item.Contains('=') && !item.Contains("CP"));
            _QN = datalist.SingleOrDefault(item => item.Contains("QN"))?.Split('=')[1] ?? throw new ArgumentException($"{GB._name} HJ212 ReissueStatistics QN Error");
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
