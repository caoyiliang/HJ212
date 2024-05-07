﻿using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadSNReq(string mn, string pw, ST st, DateTime dataTime, string polId, string sn) : IAsyncRequest
    {
        private readonly string _QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        public byte[]? Check()
        {
            return Encoding.ASCII.GetBytes(_QN);
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={_QN};ST={(int)st};CN=3019;PW={pw};MN={mn};Flag=5;CP=&&DataTime={dataTime:yyyyMMddHHmmss};PolId={polId};{polId}-SN={sn}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
