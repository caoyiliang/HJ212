﻿using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;
using Utils;

namespace HJ212.Response
{
    internal class GetHourDataRsp : IAsyncResponse<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo)>
    {
        private DateTime _beginTime;
        private DateTime _endTime;
        private readonly RspInfo _rspInfo = new();
        public async Task AnalyticalData(byte[] bytes)
        {
            var str = Encoding.ASCII.GetString(bytes.Skip(6).ToArray());
            var datalist = str.Split([";", ",", "&&"], StringSplitOptions.RemoveEmptyEntries).Where(item => item.Contains('=') && !item.Contains("CP"));
            _rspInfo.QN = datalist.FirstOrDefault(item => item.Contains("QN"));
            _rspInfo.ST = datalist.FirstOrDefault(item => item.Contains("ST"));
            _rspInfo.PW = datalist.FirstOrDefault(item => item.Contains("PW"));
            _rspInfo.MN = datalist.FirstOrDefault(item => item.Contains("MN"));
            if (!DateTime.TryParseExact(datalist.SingleOrDefault(item => item.Contains("BeginTime"))?.Split('=')[1], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out _beginTime))
            {
                throw new ArgumentException($"HJ212 Get HourData BeginTime Error");
            }
            if (!DateTime.TryParseExact(datalist.SingleOrDefault(item => item.Contains("EndTime"))?.Split('=')[1], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out _endTime))
            {
                throw new ArgumentException($"HJ212 Get HourData EndTime Error");
            }
            await Task.CompletedTask;
        }

        public (bool Type, byte[]? CheckBytes) Check(byte[] bytes)
        {
            var rs = Encoding.ASCII.GetString(bytes).Split(';');
            return (rs.Where(item => item.Contains($"CN={(int)CN_Server.取污染物小时数据}")).Any(), default);
        }

        public (DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) GetResult()
        {
            return (_beginTime, _endTime, _rspInfo);
        }
    }
}
