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
            var rs = $"QN={_QN};ST={(int)st};CN={(int)CN_Client.上传设备唯一标识};PW={pw};MN={mn};Flag={1 | (int)GB._version};CP=&&DataTime={dataTime:yyyyMMddHHmmss};PolId={polId};{polId}-SN={sn}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
