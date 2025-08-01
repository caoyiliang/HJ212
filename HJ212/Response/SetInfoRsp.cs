﻿using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Response
{
    internal class SetInfoRsp : IAsyncResponse<(string PolId, string InfoId, string Info, RspInfo RspInfo)>
    {
        private string _polId = null!;
        private string _infoId = null!;
        private string _info = null!;
        private RspInfo _rspInfo = new();
        public async Task AnalyticalData(byte[] bytes)
        {
            var str = Encoding.ASCII.GetString(bytes.Skip(6).ToArray());
            var datalist = str.Split([";", ",", "&&"], StringSplitOptions.RemoveEmptyEntries).Where(item => item.Contains('=') && !item.Contains("CP"));
            _rspInfo.QN = datalist.FirstOrDefault(item => item.Contains("QN"));
            _rspInfo.ST = datalist.FirstOrDefault(item => item.Contains("ST"));
            _rspInfo.PW = datalist.FirstOrDefault(item => item.Contains("PW"));
            _rspInfo.MN = datalist.FirstOrDefault(item => item.Contains("MN"));
            _polId = datalist.SingleOrDefault(item => item.Contains("PolId"))?.Split('=')[1] ?? throw new ArgumentException($"HJ212 Set Info PolId Error");
            _infoId = datalist.SingleOrDefault(item => item.Contains("InfoId"))?.Split('=')[1] ?? throw new ArgumentException($"HJ212 Set Info InfoId Error");
            _info = datalist.SingleOrDefault(item => item.Contains($"{_infoId}-Info"))?.Split('=')[1] ?? throw new ArgumentException($"HJ212 Set Info {_infoId}-Info Error");
            await Task.CompletedTask;
        }

        public (bool Type, byte[]? CheckBytes) Check(byte[] bytes)
        {
            var rs = Encoding.ASCII.GetString(bytes).Split(';');
            return (rs.Where(item => item.Contains($"CN={(int)CN_Server.设置现场机参数}")).Any(), default);
        }

        public (string PolId, string InfoId, string Info, RspInfo RspInfo) GetResult()
        {
            return (_polId, _infoId, _info, _rspInfo);
        }
    }
}
