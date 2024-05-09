using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadRunningTimeDataReq(string mn, string pw, ST st, DateTime dataTime, List<RunningTimeData> data, bool returnValue = true) : IAsyncRequest
    {
        private readonly string _QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        public byte[]? Check()
        {
            return Encoding.ASCII.GetBytes(_QN);
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={_QN};ST={(int)st};CN=2021;PW={pw};MN={mn};Flag={(returnValue ? 1 : 0) | (int)GB._version};CP=&&DataTime={dataTime:yyyyMMddHHmmss};{string.Join(";", data.Select(c => $"{c.Name}-RT={c.RT}"))}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
