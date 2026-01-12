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
using Utils;

namespace HJ212
{
    /// <inheritdoc/>
    public class GB : IGB
    {
        private static readonly ILogger _logger = Logs.LogFactory.GetLogger<GB>();
        private readonly PigeonPort _pigeonPort;
        private bool _isConnect = false;
        /// <inheritdoc/>
        public bool IsConnect => _isConnect;

        /// <inheritdoc/>
        public string? MN { get; set; }
        /// <inheritdoc/>
        public string PW { get; set; }
        /// <inheritdoc/>
        public bool QN { get; set; }
        /// <inheritdoc/>
        public ST ST { get; set; }
        /// <inheritdoc/>
        public Version Version { get => _version; set => _version = value; }
        /// <inheritdoc/>
        public bool CR { get => _cr; set => _cr = value; }
        /// <inheritdoc/>
        public bool LF { get => _lf; set => _lf = value; }

        internal string _name = "HJ212";
        private readonly bool _ascii;
        internal Version _version;
        internal bool _cr;
        internal bool _lf;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<(int OverTime, int ReCount, RspInfo RspInfo)>? OnSetOverTimeAndReCount;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<(string? DataLoggerId, RspInfo RspInfo)>? OnSetMN;
        /// <inheritdoc/>
        public event ActivelyAskDataEventHandler<(string? PolId, RspInfo RspInfo), DateTime?>? OnGetSystemTime;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<(string? PolId, DateTime SystemTime, RspInfo RspInfo)>? OnSetSystemTime;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<(string SKCreateTime, string NewSK, RspInfo RspInfo)>? OnSetNewSK;
        /// <inheritdoc/>
        public event ActivelyAskDataEventHandler<RspInfo, int>? OnGetRealTimeDataInterval;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<(int RtdInterval, RspInfo RspInfo)>? OnSetRealTimeDataInterval;
        /// <inheritdoc/>
        public event ActivelyAskDataEventHandler<RspInfo, int>? OnGetMinuteDataInterval;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<(int MinInterval, RspInfo RspInfo)>? OnSetMinuteDataInterval;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<(string NewPW, RspInfo RspInfo)>? OnSetNewPW;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<RspInfo>? OnStartRealTimeData;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<RspInfo>? OnStopRealTimeData;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<RspInfo>? OnStartRunningStateData;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<RspInfo>? OnStopRunningStateData;
        /// <inheritdoc/>
        public event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), (List<HistoryData> HistoryDatas, bool ReturnValue, int? Timeout)>? OnGetMinuteData;
        /// <inheritdoc/>
        public event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), (List<HistoryData> HistoryDatas, bool ReturnValue, int? Timeout)>? OnGetHourData;
        /// <inheritdoc/>
        public event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), (List<HistoryData> HistoryDatas, bool ReturnValue, int? Timeout)>? OnGetDayData;
        /// <inheritdoc/>
        public event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), List<HistoryRawData>>? OnGetRawData;
        /// <inheritdoc/>
        public event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), List<RunningTimeHistory>>? OnGetRunningTimeData;
        /// <inheritdoc/>
        public event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), List<ElectricityMonitoringHistory>>? OnGetElectricityMonitoringRealTimeData;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<(string PolId, CalibrationType? CalibrationType, RspInfo RspInfo)>? OnCalibrate;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<(string PolId, RspInfo RspInfo)>? OnRealTimeSampling;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<(string PolId, RspInfo RspInfo)>? OnStartCleaningOrBlowback;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<(string PolId, RspInfo RspInfo)>? OnComparisonSampling;
        /// <inheritdoc/>
        public event ActivelyAskDataEventHandler<RspInfo, (DateTime DataTime, int VaseNo)>? OnOutOfStandardRetentionSample;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<(string PolId, TimeOnly CstartTime, int Ctime, RspInfo RspInfo)>? OnSetSamplingPeriod;
        /// <inheritdoc/>
        public event ActivelyAskDataEventHandler<(string PolId, RspInfo RspInfo), (TimeOnly CstartTime, int Ctime)>? OnGetSamplingPeriod;
        /// <inheritdoc/>
        public event ActivelyAskDataEventHandler<(string PolId, RspInfo RspInfo), string>? OnGetSampleExtractionTime;
        /// <inheritdoc/>
        public event ActivelyAskDataEventHandler<(string PolId, RspInfo RspInfo), string>? OnGetSN;
        /// <inheritdoc/>
        public event ActivelyAskDataEventHandler<(string? PolId, DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), List<LogInfo>>? OnGetLogInfos;
        /// <inheritdoc/>
        public event ActivelyAskDataEventHandler<(string PolId, string InfoId, RspInfo RspInfo), (DateTime DataTime, List<DeviceInfo> DeviceInfos)>? OnGetInfo;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<(string PolId, string InfoId, string Info, RspInfo RspInfo)>? OnSetInfo;
        /// <inheritdoc/>
        public event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), List<AutoStandardCheckData>>? OnGetAutoStandardCheckData;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<(string PolId, RspInfo RspInfo)>? OnStartAutoStandardCheck;
        /// <inheritdoc/>
        public event ActivelyPushDataEventHandler<(string PolId, RspInfo RspInfo)>? OnStartRealTimeSampleTask;

        /// <inheritdoc/>
        public event DisconnectEventHandler? OnDisconnect { add => _pigeonPort.OnDisconnect += value; remove => _pigeonPort.OnDisconnect -= value; }
        /// <inheritdoc/>
        public event ConnectEventHandler? OnConnect { add => _pigeonPort.OnConnect += value; remove => _pigeonPort.OnConnect -= value; }
        /// <inheritdoc/>
        public event RequestedLogEventHandler? OnSentData { add => _pigeonPort.OnSentData += value; remove => _pigeonPort.OnSentData -= value; }
        /// <inheritdoc/>
        public event RespondedLogEventHandler? OnReceivedData { add => _pigeonPort.OnReceivedData += value; remove => _pigeonPort.OnReceivedData -= value; }
        /// <inheritdoc/>
        public GB(string name, IPhysicalPort physicalPort, string? mn = null, string pw = "123456", bool qn = true, ST st = ST.大气环境污染源, Version version = Version.HJT212_2017, bool CR = true, bool LF = true, bool asciiLog = true)
        {
            _name = name;
            MN = mn;
            PW = pw;
            QN = qn;
            ST = st;
            Version = version;
            this.CR = CR;
            this.LF = LF;
            _ascii = asciiLog;
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
            })))
            {
                CheckEvent = async bytes =>
                {
                    var data = bytes.Skip(6).ToArray();
                    var dstr = Encoding.ASCII.GetString(data);
                    return await Task.FromResult(StringByteUtils.BytesToString(CRC.GBcrc16(data, data.Length - 4)).Replace(" ", "") == dstr[^4..]);
                }
            };
            _pigeonPort.OnDisconnect += PigeonPort_OnDisconnect;
            _pigeonPort.OnConnect += PigeonPort_OnConnect;
            _pigeonPort.OnSentData += PigeonPort_OnSentData;
            _pigeonPort.OnReceivedData += PigeonPort_OnReceivedData;
        }

        private async Task PigeonPort_OnReceivedData(byte[] data)
        {
            _logger.Trace($"{_name} GB Rec:<-- {(_ascii ? Encoding.ASCII.GetString(data) : StringByteUtils.BytesToString(data))}");
            await Task.CompletedTask;
        }

        private async Task PigeonPort_OnSentData(byte[] data)
        {
            _logger.Trace($"{_name} GB Sent:<-- {(_ascii ? Encoding.ASCII.GetString(data) : StringByteUtils.BytesToString(data))}");
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

        /// <inheritdoc/>
        public Task CloseAsync(bool closePhysicalPort)
        {
            return _pigeonPort.StopAsync();
        }

        /// <inheritdoc/>
        public Task OpenAsync() => _pigeonPort.StartAsync();

        internal string GetGbCmd(string rs)
        {
            var brs = Encoding.ASCII.GetBytes(rs);
            return $"##{rs.Length.ToString().PadLeft(4, '0')}{rs}{StringByteUtils.BytesToString(CRC.GBcrc16(brs, brs.Length)).Replace(" ", "")}{(_cr ? "\r" : "")}{(_lf ? "\n" : "")}";
        }

#pragma warning disable IDE0051 // 删除未使用的私有成员
        #region c1
        private async Task SetOverTimeAndReCountRspEvent((int OverTime, int ReCount, RspInfo RspInfo) rs)
        {
            if (OnSetOverTimeAndReCount is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnSetOverTimeAndReCount(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB SetOverTimeAndReCount Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c2
        /// <inheritdoc/>
        public async Task UploadHardwareSN(string dataLoggerId, string cpuId, string mac1, string mac2, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadHardwareSNReq, CN9013Rsp>(new UploadHardwareSNReq(MN, PW, ST, dataLoggerId, cpuId, mac1, mac2, _version, GetGbCmd), timeout);
        }
        #endregion

        #region c3
        private async Task SetMNRspEvent((string? DataLoggerId, RspInfo RspInfo) rs)
        {
            if (OnSetMN is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnSetMN(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB SetMN Error\n{t.Exception}");
                    }
                    else
                    {
                        MN = rs.RspInfo.MN;
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c4
        private async Task GetSystemTimeRspEvent((string? PolId, RspInfo RspInfo) rs)
        {
            if (OnGetSystemTime is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnGetSystemTime(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetSystemTime Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new UploadSystemTimeReq(rs.PolId, t.Result, rs.RspInfo, _version, GetGbCmd));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c5
        private async Task SetSystemTimeRspEvent((string? PolId, DateTime SystemTime, RspInfo RspInfo) rs)
        {
            if (OnSetSystemTime is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnSetSystemTime(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB SetSystemTime Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c6
        /// <inheritdoc/>
        public async Task AskSetSystemTime(string polId, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<AskSetSystemTimeReq, CN9013Rsp>(new AskSetSystemTimeReq(MN, PW, ST, polId, _version, GetGbCmd), timeout);
        }
        #endregion

        #region c7
        /// <inheritdoc/>
        public async Task AskNewSK(string? mSKCreateTime = null, int timeout = -1)
        {
            if (string.IsNullOrEmpty(MN))
            {
                throw new ArgumentException("MN cannot be null or empty when requesting a new password.");
            }
            await _pigeonPort.RequestAsync<AskNewSKReq, CN9013Rsp>(new AskNewSKReq(MN, PW, ST, mSKCreateTime ?? DateTime.Now.ToString("yyyyMMddHHmmssfff"), _version, GetGbCmd), timeout);
        }
        #endregion

        #region c8
        private async Task SetNewSKRspEvent((string SKCreateTime, string NewSK, RspInfo RspInfo) rs)
        {
            if (OnSetNewSK is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnSetNewSK(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB SetNewSK Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c9
        private async Task GetRealTimeDataIntervalRspEvent(RspInfo rs)
        {
            if (OnGetRealTimeDataInterval is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs, _version, GetGbCmd));
                await OnGetRealTimeDataInterval(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetRealTimeDataInterval Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new GetRealTimeDataIntervalReq(t.Result, rs, _version, GetGbCmd));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c10
        private async Task SetRealTimeDataIntervalRspEvent((int RtdInterval, RspInfo RspInfo) rs)
        {
            if (OnSetRealTimeDataInterval is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnSetRealTimeDataInterval(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB SetRealTimeDataInterval Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c11
        private async Task GetMinuteDataIntervalRspEvent(RspInfo rs)
        {
            if (OnGetMinuteDataInterval is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs, _version, GetGbCmd));
                await OnGetMinuteDataInterval(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetMinuteDataInterval Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new UploadMinuteDataIntervalReq(t.Result, rs, _version, GetGbCmd));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c12
        private async Task SetMinuteDataIntervalRspEvent((int RtdInterval, RspInfo RspInfo) rs)
        {
            if (OnSetMinuteDataInterval is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnSetMinuteDataInterval(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB SetMinuteDataInterval Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c13
        private async Task SetNewPWRspEvent((string NewPW, RspInfo RspInfo) rs)
        {
            if (OnSetNewPW is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnSetNewPW(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB SetNewPW Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c14
        private async Task StartRealTimeDataRspEvent(RspInfo rs)
        {
            if (OnStartRealTimeData is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs, _version, GetGbCmd));
                await OnStartRealTimeData(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB StartRealTimeData Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c15
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
                        await _pigeonPort.SendAsync(new MessageResponseReq(rs, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c16
        private async Task StartRunningStateDataRspEvent(RspInfo rs)
        {
            if (OnStartRunningStateData is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs, _version, GetGbCmd));
                await OnStartRunningStateData(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB StartRunningStateData Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c17
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
                        await _pigeonPort.SendAsync(new MessageResponseReq(rs, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c18、29
        /// <inheritdoc/>
        public async Task UploadRealTimeData(DateTime dataTime, List<RealTimeData> data, DateTime? sendTime = null, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadRealTimeDataReq, CN9014Rsp>(new UploadRealTimeDataReq(MN, PW, ST, dataTime, data, _version, GetGbCmd, sendTime), timeout);
        }

        /// <inheritdoc/>
        public async Task SendRealTimeData(DateTime dataTime, List<RealTimeData> data, DateTime? sendTime = null)
        {
            await _pigeonPort.SendAsync(new SendRealTimeDataReq(MN, PW, QN, ST, dataTime, data, _version, GetGbCmd, sendTime));
        }
        #endregion

        #region c19
        /// <inheritdoc/>
        public async Task UploadRunningStateData(DateTime dataTime, List<RunningStateData> data, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadRunningStateDataReq, CN9014Rsp>(new UploadRunningStateDataReq(MN, PW, ST, dataTime, data, _version, GetGbCmd), timeout);
        }
        #endregion

        #region c20
        /// <inheritdoc/>
        public async Task UploadMinuteData(DateTime dataTime, List<StatisticsData> data, DateTime? sendTime = null, int reTryCount = 0, int timeout = -1, int pnum = 1, int pno = 1, CancellationToken cancellationToken = default)
        {
            var uploadStatisticsDataReq = new UploadStatisticsDataReq(CN_Client.上传污染物分钟数据, MN, PW, ST, dataTime, data, pnum, pno, _version, GetGbCmd, sendTime);
            var func = async () => await _pigeonPort.RequestAsync<UploadStatisticsDataReq, CN9014Rsp>(uploadStatisticsDataReq, timeout);

            try
            {
                await func.ReTry(reTryCount, cancellationToken);
            }
            catch (Exception ex)
            {
                ex.Data.Add("SendCmd", uploadStatisticsDataReq.ToString());
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task SendMinuteData(DateTime dataTime, List<StatisticsData> data, DateTime? sendTime = null, int pnum = 1, int pno = 1)
        {
            await _pigeonPort.SendAsync(new SendStatisticsDataReq(CN_Client.上传污染物分钟数据, MN, PW, QN, ST, dataTime, data, pnum, pno, _version, GetGbCmd, sendTime));
        }

        /// <inheritdoc/>
        public string GetSendMinuteDataCmd(DateTime dataTime, List<StatisticsData> data, DateTime? sendTime = null, int pnum = 1, int pno = 1)
        {
            var sendStatisticsDataReq = new SendStatisticsDataReq(CN_Client.上传污染物分钟数据, MN, PW, QN, ST, dataTime, data, pnum, pno, _version, GetGbCmd, sendTime);
            return Encoding.ASCII.GetString(sendStatisticsDataReq.ToBytes());
        }
        #endregion

        #region c21
        /// <inheritdoc/>
        public async Task UploadHourData(DateTime dataTime, List<StatisticsData> data, DateTime? sendTime = null, int reTryCount = 0, int timeout = -1, int pnum = 1, int pno = 1, CancellationToken cancellationToken = default)
        {
            var uploadStatisticsDataReq = new UploadStatisticsDataReq(CN_Client.上传污染物小时数据, MN, PW, ST, dataTime, data, pnum, pno, _version, GetGbCmd, sendTime);
            var func = async () => await _pigeonPort.RequestAsync<UploadStatisticsDataReq, CN9014Rsp>(uploadStatisticsDataReq, timeout);

            try
            {
                await func.ReTry(reTryCount, cancellationToken);
            }
            catch (Exception ex)
            {
                ex.Data.Add("SendCmd", uploadStatisticsDataReq.ToString());
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task SendHourData(DateTime dataTime, List<StatisticsData> data, DateTime? sendTime = null, int pnum = 1, int pno = 1)
        {
            await _pigeonPort.SendAsync(new SendStatisticsDataReq(CN_Client.上传污染物小时数据, MN, PW, QN, ST, dataTime, data, pnum, pno, _version, GetGbCmd, sendTime));
        }

        /// <inheritdoc/>
        public string GetSendHourDataCmd(DateTime dataTime, List<StatisticsData> data, DateTime? sendTime = null, int pnum = 1, int pno = 1)
        {
            var sendStatisticsDataReq = new SendStatisticsDataReq(CN_Client.上传污染物小时数据, MN, PW, QN, ST, dataTime, data, pnum, pno, _version, GetGbCmd, sendTime);
            return Encoding.ASCII.GetString(sendStatisticsDataReq.ToBytes());
        }
        #endregion

        #region c22
        /// <inheritdoc/>
        public async Task UploadDayData(DateTime dataTime, List<StatisticsData> data, DateTime? sendTime = null, int reTryCount = 0, int timeout = -1, int pnum = 1, int pno = 1, CancellationToken cancellationToken = default)
        {
            var uploadStatisticsDataReq = new UploadStatisticsDataReq(CN_Client.上传污染物日历史数据, MN, PW, ST, dataTime, data, pnum, pno, _version, GetGbCmd, sendTime);
            var func = async () => await _pigeonPort.RequestAsync<UploadStatisticsDataReq, CN9014Rsp>(uploadStatisticsDataReq, timeout);

            try
            {
                await func.ReTry(reTryCount, cancellationToken);
            }
            catch (Exception ex)
            {
                ex.Data.Add("SendCmd", uploadStatisticsDataReq.ToString());
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task SendDayData(DateTime dataTime, List<StatisticsData> data, DateTime? sendTime = null, int pnum = 1, int pno = 1)
        {
            await _pigeonPort.SendAsync(new SendStatisticsDataReq(CN_Client.上传污染物日历史数据, MN, PW, QN, ST, dataTime, data, pnum, pno, _version, GetGbCmd, sendTime));
        }

        /// <inheritdoc/>
        public string GetSendDayDataCmd(DateTime dataTime, List<StatisticsData> data, DateTime? sendTime = null, int pnum = 1, int pno = 1)
        {
            var sendStatisticsDataReq = new SendStatisticsDataReq(CN_Client.上传污染物日历史数据, MN, PW, QN, ST, dataTime, data, pnum, pno, _version, GetGbCmd, sendTime);
            return Encoding.ASCII.GetString(sendStatisticsDataReq.ToBytes());
        }
        #endregion

        #region c23
        /// <inheritdoc/>
        public async Task UploadRunningTimeData(DateTime dataTime, List<RunningTimeData> data, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadRunningTimeDataReq, CN9014Rsp>(new UploadRunningTimeDataReq(MN, PW, ST, dataTime, data, _version, GetGbCmd), timeout);
        }
        #endregion

        #region c24、55、56、57、58
        private async Task GetMinuteDataRspEvent((DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) rs)
        {
            if (OnGetMinuteData is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnGetMinuteData(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetMinuteData Error\n{t.Exception}");
                    }
                    else
                    {
                        var count = t.Result.HistoryDatas.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (t.Result.ReturnValue)
                            {
                                await UploadMinuteData(t.Result.HistoryDatas[i].DataTime, t.Result.HistoryDatas[i].Data, null, 0, t.Result.Timeout ?? -1, count, i + 1);
                            }
                            else
                            {
                                await SendMinuteData(t.Result.HistoryDatas[i].DataTime, t.Result.HistoryDatas[i].Data, null, count, i + 1);
                            }
                        }
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c25、55、56、57、58
        private async Task GetHourDataRspEvent((DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) rs)
        {
            if (OnGetHourData is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnGetHourData(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetHourData Error\n{t.Exception}");
                    }
                    else
                    {
                        var count = t.Result.HistoryDatas.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (t.Result.ReturnValue)
                            {
                                await UploadHourData(t.Result.HistoryDatas[i].DataTime, t.Result.HistoryDatas[i].Data, null, 0, t.Result.Timeout ?? -1, count, i + 1);
                            }
                            else
                            {
                                await SendHourData(t.Result.HistoryDatas[i].DataTime, t.Result.HistoryDatas[i].Data, null, count, i + 1);
                            }
                        }
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c26、55、56、57、58
        private async Task GetDayDataRspEvent((DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) rs)
        {
            if (OnGetDayData is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnGetDayData(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetDayData Error\n{t.Exception}");
                    }
                    else
                    {
                        var count = t.Result.HistoryDatas.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (t.Result.ReturnValue)
                            {
                                await UploadDayData(t.Result.HistoryDatas[i].DataTime, t.Result.HistoryDatas[i].Data, null, 0, t.Result.Timeout ?? -1, count, i + 1);
                            }
                            else
                            {
                                await SendDayData(t.Result.HistoryDatas[i].DataTime, t.Result.HistoryDatas[i].Data, null, count, i + 1);
                            }
                        }
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c27
        private async Task GetRawDataRspEvent((DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) rs)
        {
            if (OnGetRawData is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnGetRawData(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetRawData Error\n{t.Exception}");
                    }
                    else
                    {
                        var count = t.Result.Count;
                        for (int i = 0; i < count; i++)
                        {
                            await _pigeonPort.SendAsync(new UploadRawDataReq(MN, PW, ST, t.Result[i].DataTime, t.Result[i].Data, _version, GetGbCmd, false, count, i + 1));
                        }
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c28
        /// <inheritdoc/>
        public async Task UploadRawData(DateTime dataTime, List<RawData> data, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadRawDataReq, CN9014Rsp>(new UploadRawDataReq(MN, PW, ST, dataTime, data, _version, GetGbCmd), timeout);
        }
        #endregion

        #region c29
        private async Task GetRunningTimeDataRspEvent((DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) rs)
        {
            if (OnGetRunningTimeData is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnGetRunningTimeData(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetRunningTimeData Error\n{t.Exception}");
                    }
                    else
                    {
                        var count = t.Result.Count;
                        for (int i = 0; i < count; i++)
                        {
                            await _pigeonPort.SendAsync(new UploadRunningTimeDataReq(MN, PW, ST, t.Result[i].DataTime, t.Result[i].Data, _version, GetGbCmd, false, count, i + 1));
                        }
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c30
        /// <inheritdoc/>
        public async Task UploadAcquisitionDeviceRestartTime(DateTime dataTime, DateTime restartTime, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadAcquisitionDeviceRestartTimeReq, CN9014Rsp>(new UploadAcquisitionDeviceRestartTimeReq(MN, PW, ST, dataTime, restartTime, _version, GetGbCmd), timeout);
        }
        #endregion

        #region c31
        /// <inheritdoc/>
        public Task UploadFurnaceTemperature(DateTime dataTime, string avgData, int timeout = -1)
        {
            return _pigeonPort.RequestAsync<UploadFurnaceTemperatureReq, CN9014Rsp>(new UploadFurnaceTemperatureReq(MN, PW, ST, dataTime, avgData, _version, GetGbCmd), timeout);
        }
        #endregion

        #region c32
        /// <inheritdoc/>
        public async Task UploadRealTimeNoiseLevel(DateTime dataTime, float noiseLevel, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadRealTimeNoiseLevelReq, CN9014Rsp>(new UploadRealTimeNoiseLevelReq(MN, PW, ST, dataTime, noiseLevel, _version, GetGbCmd), timeout);
        }
        #endregion

        #region c33
        /// <inheritdoc/>
        public async Task UploadMinuteNoiseLevel(DateTime dataTime, List<NoiseLevelData> data, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadMinuteNoiseLevelReq, CN9014Rsp>(new UploadMinuteNoiseLevelReq(MN, PW, ST, dataTime, data, _version, GetGbCmd), timeout);
        }
        #endregion

        #region (2025中已经删除，2017-C27)
        /// <inheritdoc/>
        public async Task UploadHourNoiseLevel(DateTime dataTime, List<NoiseLevelData> data, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadHourNoiseLevelReq, CN9014Rsp>(new UploadHourNoiseLevelReq(MN, PW, ST, dataTime, data, _version, GetGbCmd), timeout);
        }
        #endregion

        #region c34
        /// <inheritdoc/>
        public Task UploadSingleNoiseLevel(DateTime dataTime, List<NoiseLevelData> data, int timeout = -1)
        {
            return _pigeonPort.RequestAsync<UploadSingleNoiseLevelReq, CN9014Rsp>(new UploadSingleNoiseLevelReq(MN, PW, ST, dataTime, data, _version, GetGbCmd), timeout);
        }
        #endregion

        #region c35
        /// <inheritdoc/>
        public async Task UploadDayNoiseLevel(DateTime dataTime, List<NoiseLevelData_Day> data, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadDayNoiseLevelReq, CN9014Rsp>(new UploadDayNoiseLevelReq(MN, PW, ST, dataTime, data, _version, GetGbCmd), timeout);
        }
        #endregion

        #region c36
        /// <inheritdoc/>
        public Task UploadKeyProductionConditionRealTimeData(DateTime dataTime, List<KeyProductionConditionData> data, int timeout = -1)
        {
            return _pigeonPort.RequestAsync<UploadKeyProductionConditionRealTimeDataReq, CN9014Rsp>(new UploadKeyProductionConditionRealTimeDataReq(MN, PW, ST, dataTime, data, _version, GetGbCmd), timeout);
        }
        #endregion

        #region c37
        /// <inheritdoc/>
        public async Task UploadElectricityMonitoringRealTimeDataForProduction(DateTime dataTime, List<ElectricityMonitoringData> data, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadElectricityMonitoringRealTimeDataReq, CN9014Rsp>(new UploadElectricityMonitoringRealTimeDataReq(MN, PW, ST, dataTime, data, _version, GetGbCmd), timeout);
        }
        #endregion

        #region c38
        /// <inheritdoc/>
        public async Task UploadElectricityMonitoringRealTimeDataForTreatment(DateTime dataTime, List<ElectricityMonitoringData> data, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadElectricityMonitoringRealTimeDataReq, CN9014Rsp>(new UploadElectricityMonitoringRealTimeDataReq(MN, PW, ST, dataTime, data, _version, GetGbCmd), timeout);
        }
        #endregion

        #region c39、c40
        private async Task GetElectricityMonitoringRealTimeDataRspEvent((DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) rs)
        {
            if (OnGetElectricityMonitoringRealTimeData is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnGetElectricityMonitoringRealTimeData(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetElectricityMonitoringRealTimeData Error\n{t.Exception}");
                    }
                    else
                    {
                        var count = t.Result.Count;
                        for (int i = 0; i < count; i++)
                        {
                            await _pigeonPort.SendAsync(new UploadElectricityMonitoringRealTimeDataReq(MN, PW, ST, t.Result[i].DataTime, t.Result[i].Data, _version, GetGbCmd, false, count, i + 1));
                        }
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c41
        private async Task CalibrateRspEvent((string PolId, CalibrationType? CalibrationType, RspInfo RspInfo) rs)
        {
            if (OnCalibrate is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnCalibrate(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB Calibrate Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c42
        private async Task RealTimeSamplingRspEvent((string PolId, RspInfo RspInfo) rs)
        {
            if (OnRealTimeSampling is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnRealTimeSampling(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB RealTimeSampling Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c43
        private async Task StartCleaningOrBlowbackRspEvent((string PolId, RspInfo RspInfo) rs)
        {
            if (OnStartCleaningOrBlowback is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnStartCleaningOrBlowback(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB StartCleaningOrBlowback Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region (2025中已经删除，2017-c33)
        private async Task ComparisonSamplingRspEvent((string PolId, RspInfo RspInfo) rs)
        {
            if (OnComparisonSampling is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnComparisonSampling(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB ComparisonSampling Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region (2025中已经删除，2017-c34)
        private async Task OutOfStandardRetentionSampleRspEvent(RspInfo rs)
        {
            if (OnOutOfStandardRetentionSample is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs, _version, GetGbCmd));
                await OnOutOfStandardRetentionSample(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB OutOfStandardRetentionSample Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new OutOfStandardRetentionSampleReq(t.Result.DataTime, t.Result.VaseNo, rs, _version, GetGbCmd));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region (2025中已经删除，2017-c35)
        private async Task SetSamplingPeriodRspEvent((string PolId, TimeOnly CstartTime, int Ctime, RspInfo RspInfo) rs)
        {
            if (OnSetSamplingPeriod is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnSetSamplingPeriod(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB SetSamplingPeriod Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region (2025中已经删除，2017-c36)
        private async Task GetSamplingPeriodRspEvent((string PolId, RspInfo RspInfo) rs)
        {
            if (OnGetSamplingPeriod is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnGetSamplingPeriod(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetSamplingPeriod Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new GetSamplingPeriodReq(rs.PolId, t.Result.CstartTime, t.Result.Ctime, rs.RspInfo, _version, GetGbCmd));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c44
        private async Task GetSampleExtractionTimeRspEvent((string PolId, RspInfo RspInfo) rs)
        {
            if (OnGetSampleExtractionTime is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnGetSampleExtractionTime(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetSampleExtractionTime Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new GetSampleExtractionTimeReq(rs.PolId, t.Result, rs.RspInfo, _version, GetGbCmd));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c45
        private async Task GetSNRspEvent((string PolId, RspInfo RspInfo) rs)
        {
            if (OnGetSN is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnGetSN(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetSN Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new GetSNReq(rs.PolId, t.Result, rs.RspInfo, _version, GetGbCmd));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c46
        /// <inheritdoc/>
        public async Task UploadSN(DateTime dataTime, string polId, string sn, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadSNReq, CN9014Rsp>(new UploadSNReq(MN, PW, ST, dataTime, polId, sn, _version, GetGbCmd), timeout);
        }
        #endregion

        #region c47
        /// <inheritdoc/>
        public async Task UploadLog(DateTime dataTime, string? polId, string log, int timeout = -1)
        {
            var logBytes = Encoding.UTF8.GetBytes(log);
            if (logBytes.Length > 890)
            {
                throw new ArgumentException("Log data exceeds 890 bytes limit.");
            }
            await _pigeonPort.RequestAsync<UploadLogReq, CN9014Rsp>(new UploadLogReq(MN, PW, ST, dataTime, polId, log, _version), timeout);
        }
        #endregion

        #region c48
        private async Task GetLogInfosRspEvent((string? PolId, DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) rs)
        {
            if (OnGetLogInfos is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnGetLogInfos(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetLogInfos Error\n{t.Exception}");
                    }
                    else
                    {
                        var count = t.Result.Count;
                        for (int i = 0; i < count; i++)
                        {
                            await _pigeonPort.SendAsync(new UploadLogReq(MN, PW, ST, t.Result[i].DataTime, t.Result[i].PolId, t.Result[i].Info, _version, count, i + 1, false));
                        }
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c49、51
        /// <inheritdoc/>
        public async Task UploadInfo(DateTime dataTime, string polId, List<DeviceInfo> deviceInfos, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadInfoReq, CN9014Rsp>(new UploadInfoReq(MN, PW, ST, dataTime, polId, deviceInfos, _version, GetGbCmd), timeout);
        }
        #endregion

        #region c50、52
        private async Task GetInfoRspEvent((string PolId, string InfoId, RspInfo RspInfo) rs)
        {
            if (OnGetInfo is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnGetInfo(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetInfo Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new UploadInfoReq(MN, PW, ST, t.Result.DataTime, rs.PolId, t.Result.DeviceInfos, _version, GetGbCmd, false));
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c53
        private async Task SetInfoRspEvent((string PolId, string InfoId, string Info, RspInfo RspInfo) rs)
        {
            if (OnSetInfo is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnSetInfo(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB SetInfo Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c54
        /// <inheritdoc/>
        public async Task SendHeartbeat(bool returnValue = true, int timeout = -1)
        {
            if (returnValue)
            {
                await _pigeonPort.RequestAsync<HeartbeatReq, CN9013Rsp>(new HeartbeatReq(MN, PW, ST, _version, GetGbCmd), timeout);
            }
            else
            {
                await _pigeonPort.SendAsync(new HeartbeatReq(MN, PW, ST, _version, GetGbCmd, false));
            }
        }
        #endregion

        #region c59
        /// <inheritdoc/>
        public async Task UploadAutoStandardCheckData(DateTime dataTime, string polId, string sampleRd, string resultType, string standardValue, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadAutoStandardCheckDataReq, CN9014Rsp>(new UploadAutoStandardCheckDataReq(MN, PW, ST, dataTime, polId, sampleRd, resultType, standardValue, _version, GetGbCmd), timeout);
        }
        #endregion

        #region c60
        private async Task GetAutoStandardCheckDataRspEvent((DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) rs)
        {
            if (OnGetAutoStandardCheckData is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnGetAutoStandardCheckData(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB GetAutoStandardCheckData Error\n{t.Exception}");
                    }
                    else
                    {
                        var count = t.Result.Count;
                        for (int i = 0; i < count; i++)
                        {
                            await _pigeonPort.RequestAsync<UploadAutoStandardCheckDataReq, CN9014Rsp>(new UploadAutoStandardCheckDataReq(MN, PW, ST, t.Result[0].DataTime, t.Result[0].PolId, t.Result[0].SampleRd, t.Result[0].ResultType, t.Result[0].StandardValue, _version, GetGbCmd, count, i + 1));
                        }
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c61
        private async Task StartAutoStandardCheckRspEvent((string PolId, RspInfo RspInfo) rs)
        {
            if (OnStartAutoStandardCheck is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnStartAutoStandardCheck(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB StartAutoStandardCheck Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c62
        private async Task StartRealTimeSampleTaskRspEvent((string PolId, RspInfo RspInfo) rs)
        {
            if (OnStartRealTimeSampleTask is not null)
            {
                await _pigeonPort.SendAsync(new ResponseReq(rs.RspInfo, _version, GetGbCmd));
                await OnStartRealTimeSampleTask(rs).ContinueWith(async t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.Error($"{_name} GB StartRealTimeSampleTask Error\n{t.Exception}");
                    }
                    else
                    {
                        await _pigeonPort.SendAsync(new SuccessfulReq(rs.RspInfo, _version, GetGbCmd));
                    }
                });
            }
        }
        #endregion

        #region c63
        /// <inheritdoc/>
        public async Task UploadSampleInfo(DateTime dataTime, string vaseNo, List<SampleInfo> sampleInfos, int timeout = -1)
        {
            await _pigeonPort.RequestAsync<UploadSampleInfoReq, CN9014Rsp>(new UploadSampleInfoReq(MN, PW, ST, dataTime, vaseNo, sampleInfos, _version, GetGbCmd), timeout);
        }
        #endregion

        /// <inheritdoc/>
        public async Task ReissueStatisticsData(string data, int reTryCount = 0, int timeout = -1, CancellationToken cancellationToken = default)
        {
            var uploadStatisticsDataReq = new UploadStatisticsDataReq(data, _name);
            var func = async () => await _pigeonPort.RequestAsync<UploadStatisticsDataReq, CN9014Rsp>(uploadStatisticsDataReq, timeout);

            try
            {
                await func.ReTry(reTryCount, cancellationToken);
            }
            catch (Exception ex)
            {
                ex.Data.Add("SendCmd", uploadStatisticsDataReq.ToString());
                throw;
            }
        }
#pragma warning restore IDE0051 // 删除未使用的私有成员
    }
}
