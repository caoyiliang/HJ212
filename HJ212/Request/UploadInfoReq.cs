using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadInfoReq(string mn, string pw, ST st, DateTime dataTime, string polId, List<DeviceInfo> deviceInfos, bool returnValue = true) : IAsyncRequest
    {
        private readonly string _QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        public byte[]? Check()
        {
            return Encoding.ASCII.GetBytes(_QN);
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={_QN};ST={(int)st};CN=3020;PW={pw};MN={mn};Flag={(returnValue ? 1 : 0) | (int)GB._version};CP=&&DataTime={dataTime:yyyyMMddHHmmss};PolId={polId};{string.Join(";", deviceInfos.Select(c => $"{c.InfoId}-Info={c.Info}"))}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
