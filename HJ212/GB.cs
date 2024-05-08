using Communication;
using Communication.Interfaces;
using HJ212.Model;
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
        private readonly string _mn;
        private readonly string _pw;
        private readonly bool _qn;
        private readonly ST _st;
        private IPigeonPort _pigeonPort;

        private bool _isConnect = false;
        public bool IsConnect => _isConnect;

        internal static string _name = "HJ212";

        public event ActivelyPushDataEventHandler<(int OverTime, int ReCount, RspInfo RspInfo)>? OnSetOverTimeAndReCount;
        public event ActivelyAskDataEventHandler<(string? PolId, RspInfo RspInfo), DateTime?>? OnGetSystemTime;
        public event ActivelyPushDataEventHandler<(string? PolId, DateTime SystemTime, RspInfo RspInfo)>? OnSetSystemTime;
        public event ActivelyAskDataEventHandler<RspInfo, int>? OnGetRealTimeDataInterval;
        public event ActivelyPushDataEventHandler<(int RtdInterval, RspInfo RspInfo)>? OnSetRealTimeDataInterval;
        public event ActivelyAskDataEventHandler<RspInfo, int>? OnGetMinuteDataInterval;
        public event ActivelyPushDataEventHandler<(int MinInterval, RspInfo RspInfo)>? OnSetMinuteDataInterval;
        public event ActivelyPushDataEventHandler<(string NewPW, RspInfo RspInfo)>? OnSetNewPW;
        public event ActivelyPushDataEventHandler<RspInfo>? OnStartRealTimeData;
        public event ActivelyPushDataEventHandler<RspInfo>? OnStopRealTimeData;
        public event ActivelyPushDataEventHandler<RspInfo>? OnStartRunningStateData;
        public event ActivelyPushDataEventHandler<RspInfo>? OnStopRunningStateData;
        public event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), (DateTime DataTime, List<StatisticsData> Data)>? OnGetMinuteData;
        public event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), (DateTime DataTime, List<StatisticsData> Data)>? OnGetHourData;
        public event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), (DateTime DataTime, List<StatisticsData> Data)>? OnGetDayData;
        public event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), (DateTime DataTime, List<RunningTimeData> Data)>? OnGetRunningTimeData;
        public event ActivelyPushDataEventHandler<(string PolId, RspInfo RspInfo)>? OnCalibrate;
        public event ActivelyPushDataEventHandler<(string PolId, RspInfo RspInfo)>? OnRealTimeSampling;
        public event ActivelyPushDataEventHandler<(string PolId, RspInfo RspInfo)>? OnStartCleaningOrBlowback;
        public event ActivelyPushDataEventHandler<(string PolId, RspInfo RspInfo)>? OnComparisonSampling;
        public event ActivelyAskDataEventHandler<RspInfo, (DateTime DataTime, int VaseNo)>? OnOutOfStandardRetentionSample;
        public event ActivelyPushDataEventHandler<(string PolId, TimeOnly CstartTime, int Ctime, RspInfo RspInfo)>? OnSetSamplingPeriod;
        public event ActivelyAskDataEventHandler<(string PolId, RspInfo RspInfo), (TimeOnly CstartTime, int Ctime)>? OnGetSamplingPeriod;
        public event ActivelyAskDataEventHandler<(string PolId, RspInfo RspInfo), int>? OnGetSampleExtractionTime;
        public event ActivelyAskDataEventHandler<(string PolId, RspInfo RspInfo), string>? OnGetSN;
        public event ActivelyAskDataEventHandler<(string? PolId, DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), List<LogInfo>>? OnGetLogInfos;
        public event ActivelyAskDataEventHandler<(string PolId, string InfoId, RspInfo RspInfo), (DateTime DataTime, List<DeviceInfo> DeviceInfos)>? OnGetInfo;

        /// <inheritdoc/>
        public event DisconnectEventHandler? OnDisconnect { add => _pigeonPort.OnDisconnect += value; remove => _pigeonPort.OnDisconnect -= value; }
        /// <inheritdoc/>
        public event ConnectEventHandler? OnConnect { add => _pigeonPort.OnConnect += value; remove => _pigeonPort.OnConnect -= value; }
        public GB(string name, IPhysicalPort physicalPort, string mn, string pw = "123456", bool qn = true, ST st = ST.大气环境污染源)
        {
            _name = name;
            _mn = mn;
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
            _pigeonPort.OnDisconnect += PigeonPort_OnDisconnect;
            _pigeonPort.OnConnect += PigeonPort_OnConnect;
            _pigeonPort.OnSentData += PigeonPort_OnSentData;
            _pigeonPort.OnReceivedData += PigeonPort_OnReceivedData;
        }

        private async Task PigeonPort_OnReceivedData(byte[] data)
        {
            _logger.Trace($"{_name} GB Rec:<-- {StringByteUtils.BytesToString(data)}");
            await Task.CompletedTask;
        }

        private async Task PigeonPort_OnSentData(byte[] data)
        {
            _logger.Trace($"{_name} GB Sent:<-- {StringByteUtils.BytesToString(data)}");
            await Task.CompletedTask;
        }

        private async Task PigeonPort_OnConnect()
        {
            _isConnect = true;
            await Task.CompletedTask;
        }

        private async Task PigeonPort_OnDisconnect()
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

        #region c1
        private async Task SetOverTimeAndReCountRspEvent((int OverTime, int ReCount, RspInfo RspInfo) rs)
        {
            if (OnSetOverTimeAndReCount is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnSetOverTimeAndReCount(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB SetOverTimeAndReCount Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c2
        private async Task GetSystemTimeRspEvent((string? PolId, RspInfo RspInfo) rs)
        {
            if (OnGetSystemTime is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnGetSystemTime(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetSystemTime Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new CN1011Req(rs.PolId, t.Result, rs.RspInfo));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c3
        private async Task SetSystemTimeRspEvent((string? PolId, DateTime SystemTime, RspInfo RspInfo) rs)
        {
            if (OnSetSystemTime is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnSetSystemTime(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB SetSystemTime Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c4
        public async Task AskSetSystemTime(string polId, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<AskSetSystemTimeReq, AskSetSystemTimeRsp>(new AskSetSystemTimeReq(_mn, _pw, _st, polId), timeout);
        }
        #endregion

        #region c5
        private async Task GetRealTimeDataIntervalRspEvent(RspInfo rs)
        {
            if (OnGetRealTimeDataInterval is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs));
                await OnGetRealTimeDataInterval(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetRealTimeDataInterval Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new GetRealTimeDataIntervalReq(t.Result, rs));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs));
                    }
                });
            }
        }
        #endregion

        #region c6
        private async Task SetRealTimeDataIntervalRspEvent((int RtdInterval, RspInfo RspInfo) rs)
        {
            if (OnSetRealTimeDataInterval is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnSetRealTimeDataInterval(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB SetRealTimeDataInterval Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c7
        private async Task GetMinuteDataIntervalRspEvent(RspInfo rs)
        {
            if (OnGetMinuteDataInterval is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs));
                await OnGetMinuteDataInterval(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetMinuteDataInterval Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new CN1063Req(t.Result, rs));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs));
                    }
                });
            }
        }
        #endregion

        #region c8
        private async Task SetMinuteDataIntervalRspEvent((int RtdInterval, RspInfo RspInfo) rs)
        {
            if (OnSetMinuteDataInterval is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnSetMinuteDataInterval(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB SetMinuteDataInterval Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c9
        private async Task SetNewPWRspEvent((string NewPW, RspInfo RspInfo) rs)
        {
            if (OnSetNewPW is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnSetNewPW(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB SetNewPW Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c10
        private async Task StartRealTimeDataRspEvent(RspInfo rs)
        {
            if (OnStartRealTimeData is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs));
                await OnStartRealTimeData(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB StartRealTimeData Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs));
                    }
                });
            }
        }
        #endregion

        #region c11
        private async Task StopRealTimeDataRspEvent(RspInfo rs)
        {
            if (OnStopRealTimeData is not null)
            {
                await OnStopRealTimeData(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB StopRealTimeData Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new MessageResponseReq(rs));
                    }
                });
            }
        }
        #endregion

        #region c12
        private async Task StartRunningStateDataRspEvent(RspInfo rs)
        {
            if (OnStartRunningStateData is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs));
                await OnStartRunningStateData(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB StartRunningStateData Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs));
                    }
                });
            }
        }
        #endregion

        #region c13
        private async Task StopRunningStateDataRspEvent(RspInfo rs)
        {
            if (OnStopRunningStateData is not null)
            {
                await OnStopRunningStateData(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB StopRunningStateData Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new MessageResponseReq(rs));
                    }
                });
            }
        }
        #endregion

        #region c14、29
        public async Task UploadRealTimeData(DateTime dataTime, List<RealTimeData> data, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadRealTimeDataReq, CN9014Rsp>(new UploadRealTimeDataReq(_mn, _pw, _st, dataTime, data), timeout);
        }

        public async Task SendRealTimeData(DateTime dataTime, List<RealTimeData> data)
        {
            await _pigeonPort.SendAsync(new SendRealTimeDataReq(_mn, _pw, _qn, _st, dataTime, data));
        }
        #endregion

        #region c15
        public async Task UploadRunningStateData(DateTime dataTime, List<RunningStateData> data, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadRunningStateDataReq, CN9014Rsp>(new UploadRunningStateDataReq(_mn, _pw, _st, dataTime, data), timeout);
        }
        #endregion

        #region c16
        public async Task UploadMinuteData(DateTime dataTime, List<StatisticsData> data, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadStatisticsDataReq, CN9014Rsp>(new UploadStatisticsDataReq(CN.分钟数据, _mn, _pw, _st, dataTime, data), timeout);
        }

        public async Task SendMinuteData(DateTime dataTime, List<StatisticsData> data)
        {
            await _pigeonPort.SendAsync(new SendStatisticsDataReq(CN.分钟数据, _mn, _pw, _qn, _st, dataTime, data));
        }
        #endregion

        #region c17
        public async Task UploadHourData(DateTime dataTime, List<StatisticsData> data, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadStatisticsDataReq, CN9014Rsp>(new UploadStatisticsDataReq(CN.小时数据, _mn, _pw, _st, dataTime, data), timeout);
        }

        public async Task SendHourData(DateTime dataTime, List<StatisticsData> data)
        {
            await _pigeonPort.SendAsync(new SendStatisticsDataReq(CN.小时数据, _mn, _pw, _qn, _st, dataTime, data));
        }
        #endregion

        #region c18
        public async Task UploadDayData(DateTime dataTime, List<StatisticsData> data, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadStatisticsDataReq, CN9014Rsp>(new UploadStatisticsDataReq(CN.日历史数据, _mn, _pw, _st, dataTime, data), timeout);
        }

        public async Task SendDayData(DateTime dataTime, List<StatisticsData> data)
        {
            await _pigeonPort.SendAsync(new SendStatisticsDataReq(CN.日历史数据, _mn, _pw, _qn, _st, dataTime, data));
        }
        #endregion

        #region c19
        public async Task UploadRunningTimeData(DateTime dataTime, List<RunningTimeData> data, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadRunningTimeDataReq, CN9014Rsp>(new UploadRunningTimeDataReq(_mn, _pw, _st, dataTime, data), timeout);
        }
        #endregion

        #region c20
        private async Task GetMinuteDataRspEvent((DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) rs)
        {
            if (OnGetMinuteData is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnGetMinuteData(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetMinuteData Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SendStatisticsDataReq(CN.分钟数据, _mn, _pw, _qn, _st, t.Result.DataTime, t.Result.Data));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c21
        private async Task GetHourDataRspEvent((DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) rs)
        {
            if (OnGetHourData is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnGetHourData(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetHourData Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SendStatisticsDataReq(CN.小时数据, _mn, _pw, _qn, _st, t.Result.DataTime, t.Result.Data));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c22
        private async Task GetDayDataRspEvent((DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) rs)
        {
            if (OnGetDayData is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnGetDayData(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetDayData Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SendStatisticsDataReq(CN.日历史数据, _mn, _pw, _qn, _st, t.Result.DataTime, t.Result.Data));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c23
        private async Task GetRunningTimeDataRspEvent((DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) rs)
        {
            if (OnGetRunningTimeData is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnGetRunningTimeData(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetRunningTimeData Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new UploadRunningTimeDataReq(_mn, _pw, _st, t.Result.DataTime, t.Result.Data));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c24
        public async Task UploadAcquisitionDeviceRestartTime(DateTime dataTime, DateTime restartTime, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadAcquisitionDeviceRestartTimeReq, CN9014Rsp>(new UploadAcquisitionDeviceRestartTimeReq(_mn, _pw, _st, dataTime, restartTime), timeout);
        }
        #endregion

        #region c25
        public async Task UploadRealTimeNoiseLevel(DateTime dataTime, float noiseLevel, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadRealTimeNoiseLevelReq, CN9014Rsp>(new UploadRealTimeNoiseLevelReq(_mn, _pw, _st, dataTime, noiseLevel), timeout);
        }
        #endregion

        #region c26
        public async Task UploadMinuteNoiseLevel(DateTime dataTime, List<NoiseLevelData> data, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadMinuteNoiseLevelReq, CN9014Rsp>(new UploadMinuteNoiseLevelReq(_mn, _pw, _st, dataTime, data), timeout);
        }
        #endregion

        #region c27
        public async Task UploadHourNoiseLevel(DateTime dataTime, List<NoiseLevelData> data, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadHourNoiseLevelReq, CN9014Rsp>(new UploadHourNoiseLevelReq(_mn, _pw, _st, dataTime, data), timeout);
        }
        #endregion

        #region c28
        public async Task UploadDayNoiseLevel(DateTime dataTime, List<NoiseLevelData_Day> data, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadDayNoiseLevelReq, CN9014Rsp>(new UploadDayNoiseLevelReq(_mn, _pw, _st, dataTime, data), timeout);
        }
        #endregion

        #region c30
        private async Task CalibrateRspEvent((string PolId, RspInfo RspInfo) rs)
        {
            if (OnCalibrate is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnCalibrate(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB Calibrate Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c31
        private async Task RealTimeSamplingRspEvent((string PolId, RspInfo RspInfo) rs)
        {
            if (OnRealTimeSampling is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnRealTimeSampling(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB RealTimeSampling Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c32
        private async Task StartCleaningOrBlowbackRspEvent((string PolId, RspInfo RspInfo) rs)
        {
            if (OnStartCleaningOrBlowback is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnStartCleaningOrBlowback(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB StartCleaningOrBlowback Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c33
        private async Task ComparisonSamplingRspEvent((string PolId, RspInfo RspInfo) rs)
        {
            if (OnComparisonSampling is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnComparisonSampling(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB ComparisonSampling Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c34
        private async Task OutOfStandardRetentionSampleRspEvent(RspInfo rs)
        {
            if (OnOutOfStandardRetentionSample is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs));
                await OnOutOfStandardRetentionSample(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB OutOfStandardRetentionSample Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new OutOfStandardRetentionSampleReq(t.Result.DataTime, t.Result.VaseNo, rs));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs));
                    }
                });
            }
        }
        #endregion

        #region c35
        private async Task SetSamplingPeriodRspEvent((string PolId, TimeOnly CstartTime, int Ctime, RspInfo RspInfo) rs)
        {
            if (OnSetSamplingPeriod is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnSetSamplingPeriod(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB SetSamplingPeriod Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c36
        private async Task GetSamplingPeriodRspEvent((string PolId, RspInfo RspInfo) rs)
        {
            if (OnGetSamplingPeriod is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnGetSamplingPeriod(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetSamplingPeriod Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new GetSamplingPeriodReq(rs.PolId, t.Result.CstartTime, t.Result.Ctime, rs.RspInfo));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c37
        private async Task GetSampleExtractionTimeRspEvent((string PolId, RspInfo RspInfo) rs)
        {
            if (OnGetSampleExtractionTime is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnGetSampleExtractionTime(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetSampleExtractionTime Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new GetSampleExtractionTimeReq(rs.PolId, t.Result, rs.RspInfo));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c38
        private async Task GetSNRspEvent((string PolId, RspInfo RspInfo) rs)
        {
            if (OnGetSN is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnGetSN(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetSN Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new GetSNReq(rs.PolId, t.Result, rs.RspInfo));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c39
        public async Task UploadSN(DateTime dataTime, string polId, string sn, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadSNReq, CN9014Rsp>(new UploadSNReq(_mn, _pw, _st, dataTime, polId, sn), timeout);
        }
        #endregion

        #region c40
        public async Task UploadLog(DateTime dataTime, string? polId, string log, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadLogReq, CN9014Rsp>(new UploadLogReq(_mn, _pw, _st, 5, dataTime, polId, log), timeout);
        }
        #endregion

        #region c41
        private async Task GetLogInfosRspEvent((string? PolId, DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) rs)
        {
            if (OnGetLogInfos is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnGetLogInfos(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetLogInfos Error\n{t.Exception}");
                    }
                    else
                    {
                        foreach (var item in t.Result)
                        {
                            await _pigeonPort.SendAsync(new UploadLogReq(_mn, _pw, _st, 4, item.DataTime, item.PolId, item.Info));
                        }
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion

        #region c42、44
        public async Task UploadInfo(DateTime dataTime, string polId, List<DeviceInfo> deviceInfos, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadInfoReq, CN9014Rsp>(new UploadInfoReq(_mn, _pw, _st, 5, dataTime, polId, deviceInfos), timeout);
        }
        #endregion

        #region c43、45
        private async Task GetInfoRspEvent((string PolId, string InfoId, RspInfo RspInfo) rs)
        {
            if (OnGetInfo is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo));
                await OnGetInfo(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetInfo Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new UploadInfoReq(_mn, _pw, _st, 4, t.Result.DataTime, rs.PolId, t.Result.DeviceInfos));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo));
                    }
                });
            }
        }
        #endregion
    }
}
