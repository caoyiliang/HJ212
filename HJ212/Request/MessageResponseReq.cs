﻿using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class MessageResponseReq(RspInfo rspInfo, Version version, Func<string, string> func) : IByteStream
    {
        public byte[] ToBytes()
        {
            var cmd = $"{rspInfo.QN};ST=91;CN={(int)CN_Client.通知应答};{rspInfo.PW};{rspInfo.MN};Flag={0 | (int)version};CP=&&&&";
            cmd = func.Invoke(cmd);
            return Encoding.ASCII.GetBytes(cmd);
        }
    }
}
