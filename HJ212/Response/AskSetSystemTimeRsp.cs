using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopPortLib.Interfaces;
using Utils;

namespace HJ212.Response
{
    internal class AskSetSystemTimeRsp : IAsyncResponse<bool>
    {
        bool _success;
        public async Task AnalyticalData(byte[] bytes)
        {
            _success = true;
            await Task.CompletedTask;
        }

        public bool Check(byte[] bytes)
        {
            var data = bytes.Skip(6).ToArray();
            var dstr = Encoding.ASCII.GetString(data);
            if (StringByteUtils.BytesToString(CRC.GBcrc16(data, data.Length - 4)).Replace(" ", "") != dstr[^4..])
            {
                throw new ArgumentException($"{GB._name} HJ212 CRC Error: {dstr}", nameof(bytes));
            }
            var rs = dstr.Split(';');
            return rs.Where(item => item.Contains("CN=9013")).Any();
        }

        public bool GetResult()
        {
            return _success;
        }
    }
}
