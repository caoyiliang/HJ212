﻿using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;
using Utils;

namespace HJ212.Response
{
    internal class CalibrateRsp : IAsyncResponse<(string PolId, RspInfo RspInfo)>
    {
        private string _polId = null!;
        private RspInfo _rspInfo = new();
        public async Task AnalyticalData(byte[] bytes)
        {
            var str = Encoding.ASCII.GetString(bytes.Skip(6).ToArray());
            var datalist = str.Split([";", ",", "&&"], StringSplitOptions.RemoveEmptyEntries).Where(item => item.Contains('=') && !item.Contains("CP"));
            _rspInfo.QN = datalist.FirstOrDefault(item => item.Contains("QN"));
            _rspInfo.ST = datalist.FirstOrDefault(item => item.Contains("ST"));
            _rspInfo.PW = datalist.FirstOrDefault(item => item.Contains("PW"));
            _rspInfo.MN = datalist.FirstOrDefault(item => item.Contains("MN"));
            _polId = datalist.SingleOrDefault(item => item.Contains("PolId"))?.Split('=')[1] ?? throw new ArgumentException($"HJ212 Calibrate Error");
            await Task.CompletedTask;
        }

        public (bool Type, byte[]? CheckBytes) Check(byte[] bytes)
        {
            var rs = Encoding.ASCII.GetString(bytes).Split(';');
            return (rs.Where(item => item.Contains($"CN={(int)CN_Server.零点校准量程校准}")).Any(), default);
        }

        public (string PolId, RspInfo RspInfo) GetResult()
        {
            return (_polId, _rspInfo);
        }
    }
}
