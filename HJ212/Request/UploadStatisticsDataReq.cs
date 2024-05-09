using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadStatisticsDataReq(CN cn, string mn, string pw, ST st, DateTime dataTime, List<StatisticsData> data, int pnum, int pno) : IAsyncRequest
    {
        private readonly string _QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        public byte[]? Check()
        {
            return Encoding.ASCII.GetBytes(_QN);
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={_QN};ST={(int)st};CN={(int)cn};PW={pw};MN={mn};Flag={(pnum > 1 ? $"{3 | (int)GB._version};PNUM={pnum};PNO={pno}" : $"{1 | (int)GB._version}")};CP=&&DataTime={dataTime:yyyyMMddHHmmss};{string.Join(";", data.Select(c => $"{(c.Cou != null ? $"{c.Name}-Cou={c.Cou}," : "")}{c.Name}-Min={c.Min},{c.Name}-Avg={c.Avg},{c.Name}-Max={c.Max}{(c.Flag != null ? $",{c.Name}-Flag={c.Flag}" : "")}"))}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
