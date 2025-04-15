﻿using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadRunningTimeDataReq(string mn, string pw, ST st, DateTime dataTime, List<RunningTimeData> data, Version version, Func<string, string> func, bool returnValue = true, int pnum = 1, int pno = 1) : IAsyncRequest
    {
        private readonly string _QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        public byte[]? Check()
        {
            return Encoding.ASCII.GetBytes(_QN);
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={_QN};ST={(int)st};CN={(int)CN_Client.上传设备运行时间日历史数据};PW={pw};MN={mn};Flag={(pnum > 1 ? $"{2 | (int)version};PNUM={pnum};PNO={pno}" : $"{(returnValue ? 1 : 0) | (int)version}")};CP=&&DataTime={dataTime:yyyyMMddHHmmss};{string.Join(";", data.Select(c => $"{c.Name}-RT={c.RT}"))}&&";
            rs = func.Invoke(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
