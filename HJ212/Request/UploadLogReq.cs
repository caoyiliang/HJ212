using System.Text;
using TopPortLib.Interfaces;
using Utils;

namespace HJ212.Request
{
    internal class UploadLogReq(string mn, string pw, ST st, DateTime dataTime, string? polId, string log, Version version, int pnum = 1, int pno = 1, bool returnValue = true) : IAsyncRequest
    {
        private readonly string _QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        public byte[]? Check()
        {
            return Encoding.ASCII.GetBytes(_QN);
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={_QN};ST={(int)st};CN={(int)CN_Client.上传现场机信息};PW={pw};MN={mn};Flag={(pnum > 1 ? $"{(returnValue ? 3 : 2) | (int)version};PNUM={pnum};PNO={pno}" : $"{(returnValue ? 1 : 0) | (int)version}")};CP=&&DataTime={dataTime:yyyyMMddHHmmss};{(polId == null ? "" : $"PolId ={polId};")}i11001-Info=//";
            var len = rs.Length + log.Length + 4;
            byte[] brs = [.. Encoding.ASCII.GetBytes(rs), .. Encoding.UTF8.GetBytes(log), .. Encoding.ASCII.GetBytes("//&&")];
            return [.. Encoding.ASCII.GetBytes($"##{len.ToString().PadLeft(4, '0')}"), .. brs, .. Encoding.ASCII.GetBytes($"{StringByteUtils.BytesToString(CRC.GBcrc16(brs, brs.Length)).Replace(" ", "")}\r\n")];
        }
    }
}
