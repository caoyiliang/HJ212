using Communication;
using Communication.Interfaces;
using HJ212.Request;
using HJ212.Response;
using LogInterface;
using Parser;
using Parser.Parsers;
using System.Text;
using TopPortLib;
using TopPortLib.Interfaces;
using Utils;

namespace HJ212
{
    public class GB : IGB
    {
        private static readonly ILogger _logger = Logs.LogFactory.GetLogger<GB>();
        private readonly string _name;
        private readonly string _mn;
        private readonly bool _flag;
        private readonly string _pw;
        private readonly bool _qn;
        private readonly ST _st;
        private IPigeonPort _pigeonPort;

        private bool _isConnect = false;
        public bool IsConnect => _isConnect;

        public event ActivelyPushDataEventHandler<(int OverTime, int ReCount, RspInfo RspInfo)>? OnSetOverTimeAndReCount;

        /// <inheritdoc/>
        public event DisconnectEventHandler? OnDisconnect { add => _pigeonPort.OnDisconnect += value; remove => _pigeonPort.OnDisconnect -= value; }
        /// <inheritdoc/>
        public event ConnectEventHandler? OnConnect { add => _pigeonPort.OnConnect += value; remove => _pigeonPort.OnConnect -= value; }
        public GB(string name, IPhysicalPort physicalPort, string mn, bool flag = true, string pw = "123456", bool qn = true, ST st = ST.大气环境污染源)
        {
            _name = name;
            _mn = mn;
            _flag = flag;
            _pw = pw;
            _qn = qn;
            _st = st;
            _pigeonPort = new PigeonPort(this, new TopPort(physicalPort, new HeadLengthParser([0x23, 0x23], d =>
            {
                if (d.Length < 15) return Task.FromResult(new GetDataLengthRsp() { StateCode = Parser.StateCode.LengthNotEnough });
                if (int.TryParse(Encoding.UTF8.GetString(d, 2, 4), out var rs))
                {
                    return Task.FromResult(new GetDataLengthRsp() { Length = rs + 8, StateCode = Parser.StateCode.Success });
                }
                else
                {
                    return Task.FromResult(new GetDataLengthRsp() { Length = 4, StateCode = Parser.StateCode.Success });
                }
            })));
            _pigeonPort.OnDisconnect += _pigeonPort_OnDisconnect;
            _pigeonPort.OnConnect += _pigeonPort_OnConnect;
            _pigeonPort.OnSentData += _pigeonPort_OnSentData;
            _pigeonPort.OnReceivedData += _pigeonPort_OnReceivedData;
        }

        private async Task _pigeonPort_OnReceivedData(byte[] data)
        {
            _logger.Trace($"{_name} GB Rec:<-- {StringByteUtils.BytesToString(data)}");
            await Task.CompletedTask;
        }

        private async Task _pigeonPort_OnSentData(byte[] data)
        {
            _logger.Trace($"{_name} GB Sent:<-- {StringByteUtils.BytesToString(data)}");
            await Task.CompletedTask;
        }

        private async Task _pigeonPort_OnConnect()
        {
            _isConnect = true;
            await Task.CompletedTask;
        }

        private async Task _pigeonPort_OnDisconnect()
        {
            _isConnect = false;
            await Task.CompletedTask;
        }

        public Task CloseAsync() => _pigeonPort.StopAsync();

        public Task OpenAsync() => _pigeonPort.StartAsync();

        internal static string GetGbCmd(string rs)
        {
            var brs = Encoding.ASCII.GetBytes(rs);
            return $"##{rs.Length.ToString().PadLeft(4, '0')}{rs}{StringByteUtils.BytesToString(CRC.GBcrc16(brs, brs.Length)).Replace(" ", "")}\r\n";
        }

        public async Task SendRealTimeData(DateTime dataTime, Dictionary<string, (string? value, string? flag)> data)
        {
            await _pigeonPort.SendAsync(new SendRealTimeDataReq(_mn, _flag, _pw, _qn, _st, dataTime, data));
        }

        public async Task SendMinuteData(DateTime dataTime, Dictionary<string, (string? avgValue, string? max, string? min, string? flag)> data)
        {
            await _pigeonPort.SendAsync(new SendMinuteDataReq(_mn, _flag, _pw, _qn, _st, dataTime, data));
        }

        public async Task SendHourData(DateTime dataTime, Dictionary<string, (string? avgValue, string? max, string? min, string? flag)> data)
        {
            await _pigeonPort.SendAsync(new SendHourDataReq(_mn, _flag, _pw, _qn, _st, dataTime, data));
        }

        public async Task SendDayData(DateTime dataTime, Dictionary<string, (string? avgValue, string? max, string? min, string? flag)> data)
        {
            await _pigeonPort.SendAsync(new SendDayDataReq(_mn, _flag, _pw, _qn, _st, dataTime, data));
        }

        private async Task SetOverTimeAndReCountRspEvent((int OverTime, int ReCount, RspInfo RspInfo) rs)
        {
            if (OnSetOverTimeAndReCount is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnSetOverTimeAndReCount(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB SetOverTimeAndReCount Error");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
    }
}
