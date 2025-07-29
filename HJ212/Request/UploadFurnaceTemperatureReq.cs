using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadFurnaceTemperatureReq(string? mn, string pw, ST st, DateTime dataTime, string avgData, Version version, Func<string, string> func) : IAsyncRequest
    {
        private readonly string _QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        public byte[]? Check()
        {
            return Encoding.ASCII.GetBytes(_QN);
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={_QN};ST={(int)st};CN={(int)CN_Client.上传炉膛温度5min均值};PW={pw};MN={mn};Flag={1 | (int)version};CP=&&DataTime={dataTime:yyyyMMddHHmmss};a01901-Avg={avgData}&&";
            rs = func.Invoke(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
