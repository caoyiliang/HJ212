﻿using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class AskSetSystemTimeReq(string mn, string pw, ST st, string polId) : IAsyncRequest
    {
        public byte[]? Check()
        {
            return default;
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={DateTime.Now:yyyyMMddHHmmssfff};ST={(int)st};CN={(int)CN_Client.现场机时间校准请求};PW={pw};MN={mn};Flag={1 | (int)GB._version};CP=&&PolId={polId}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
