using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadAcquisitionDeviceRestartTimeReq(string mn, string pw, ST st, DateTime dataTime, DateTime restartTime, Version version, Func<string, string> func) : IAsyncRequest
    {
        private readonly string _QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        public byte[]? Check()
        {
            return Encoding.ASCII.GetBytes(_QN);
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={_QN};ST={(int)st};CN={(int)CN_Client.上传数采仪开机时间};PW={pw};MN={mn};Flag={1 | (int)version};CP=&&DataTime={dataTime:yyyyMMddHHmmss};RestartTime={restartTime:yyyyMMddHHmmss}&&";
            rs = func.Invoke(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
