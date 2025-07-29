using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Response
{
    internal class CN9013Rsp : IAsyncResponse<bool>
    {
        bool _success;
        public async Task AnalyticalData(byte[] bytes)
        {
            _success = true;
            await Task.CompletedTask;
        }

        public (bool Type, byte[]? CheckBytes) Check(byte[] bytes)
        {
            var rs = Encoding.ASCII.GetString(bytes).Split(';');
            return (rs.Where(item => item.Contains($"CN={(int)CN_Server.通知应答}")).Any(), default);
        }

        public bool GetResult()
        {
            return _success;
        }
    }
}
