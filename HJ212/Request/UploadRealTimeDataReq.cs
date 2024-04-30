﻿using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadRealTimeDataReq(string mn, string pw, ST st, DateTime dataTime, List<RealTimeData> data) : IAsyncRequest
    {
        private readonly string _QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        public byte[]? Check()
        {
            return Encoding.ASCII.GetBytes(_QN);
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={_QN};ST={(int)st};CN={(int)CN.实时数据};PW={pw};MN={mn};CP=&&DataTime={dataTime:yyyyMMddHHmmss};{string.Join(";", data.Select(c => $"{(c.SampleTime != null ? $"{c.Name}-SampleTime={c.SampleTime}," : "")}{c.Name}-Rtd={c.Rtd}{(c.Flag != null ? $",{c.Name}-Flag={c.Flag}" : "")}{(c.EFlag != null ? $",{c.Name}-EFlag={c.EFlag}" : "")}"))}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
