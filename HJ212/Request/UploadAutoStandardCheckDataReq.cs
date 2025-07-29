using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadAutoStandardCheckDataReq(string? mn, string pw, ST st, DateTime dataTime, string polId, string? sampleRd, string? resultType, string? standardValue, Version version, Func<string, string> func, int pnum = 1, int pno = 1) : IAsyncRequest
    {
        private readonly string _QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        public byte[]? Check()
        {
            return Encoding.ASCII.GetBytes(_QN);
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={_QN};ST={(int)st};CN={(int)CN_Client.上传自动标样核查校准数据};PW={pw};MN={mn};Flag={(pnum > 1 ? $"{3 | (int)version};PNUM={pnum};PNO={pno}" : $"{1 | (int)version}")};CP=&&DataTime={dataTime:yyyyMMddHHmmss};{polId}-SampleRd={sampleRd};{polId}-StandardValue={standardValue};{polId}-ResultType={resultType}&&";
            rs = func.Invoke(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
