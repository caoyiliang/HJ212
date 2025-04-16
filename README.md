# HJ212国标协议：

``` C#
IGB gb = new GB("输出一", new SerialPort(), "88888888");
//比如上传实时数据
await gb.UploadRealTimeData(DateTime.Now, [new("a1001") { Rtd = "20.5", Flag = "N" }]);

//若系统想支持国标的平台来获取时间，只需要加上如下代码即可
gb.OnGetSystemTime += Gb_OnGetSystemTime;

async Task<DateTime?> Gb_OnGetSystemTime((string? PolId, HJ212.Model.RspInfo RspInfo) objects)
{
    return await Task.FromResult(DateTime.Now);
}
```
## 更多详见Test例子，例子中附带测试命令

# 实现HJ212-2017《污染源在线自动监控（监测）系统数据传输标准》中 附 录 C（资料性附录）通讯命令示例和拆分包及应答机制示例 中所有（表1-表50）详细接口如下，若有错误和建议请及时联系我改正：

``` C#
public interface IGB : IProtocol
{
    /// <summary>C1设置超时时间及重发次数</summary>
    event ActivelyPushDataEventHandler<(int OverTime, int ReCount, RspInfo RspInfo)> OnSetOverTimeAndReCount;

    /// <summary>C2提取现场机时间</summary>
    event ActivelyAskDataEventHandler<(string? PolId, RspInfo RspInfo), DateTime?> OnGetSystemTime;

    /// <summary>C3设置现场机时间</summary>
    event ActivelyPushDataEventHandler<(string? PolId, DateTime SystemTime, RspInfo RspInfo)> OnSetSystemTime;

    /// <summary>C4现场机时间校准请求</summary>
    Task AskSetSystemTime(string polId, int timeout = -1);

    /// <summary>C5提取实时数据间隔</summary>
    event ActivelyAskDataEventHandler<RspInfo, int> OnGetRealTimeDataInterval;

    /// <summary>C6设置实时数据间隔</summary>
    event ActivelyPushDataEventHandler<(int RtdInterval, RspInfo RspInfo)> OnSetRealTimeDataInterval;

    /// <summary>C7提取分钟数据间隔</summary>
    event ActivelyAskDataEventHandler<RspInfo, int> OnGetMinuteDataInterval;

    /// <summary>C8设置分钟数据间隔</summary>
    event ActivelyPushDataEventHandler<(int MinInterval, RspInfo RspInfo)> OnSetMinuteDataInterval;

    /// <summary>C9设置现场机访问密码</summary>
    event ActivelyPushDataEventHandler<(string NewPW, RspInfo RspInfo)> OnSetNewPW;

    /// <summary>C10取污染物实时数据</summary>
    event ActivelyPushDataEventHandler<RspInfo> OnStartRealTimeData;

    /// <summary>C11停止察看污染物实时数据</summary>
    event ActivelyPushDataEventHandler<RspInfo> OnStopRealTimeData;

    /// <summary>C12取设备运行状态数据</summary>
    event ActivelyPushDataEventHandler<RspInfo> OnStartRunningStateData;

    /// <summary>C13停止察看设备运行状态</summary>
    event ActivelyPushDataEventHandler<RspInfo> OnStopRunningStateData;

    /// <summary>
    /// C14上传污染物实时数据
    /// (C29上传工况实时数据 同)
    /// </summary>
    Task UploadRealTimeData(DateTime dataTime, List<RealTimeData> data, int timeout = -1);

    /// <summary>C14上传污染物实时数据(无返回变体)</summary>
    Task SendRealTimeData(DateTime dataTime, List<RealTimeData> data);

    /// <summary>C15上传设备运行状态数据</summary>
    Task UploadRunningStateData(DateTime dataTime, List<RunningStateData> data, int timeout = -1);

    /// <summary>C16上传污染物分钟数据</summary>
    Task UploadMinuteData(DateTime dataTime, List<StatisticsData> data, int reTryCount = 0, int timeout = -1, int pnum = 1, int pno = 1, CancellationToken cancellationToken = default);

    /// <summary>C16上传污染物分钟数据(无返回变体)</summary>
    Task SendMinuteData(DateTime dataTime, List<StatisticsData> data, int pnum = 1, int pno = 1);

    /// <summary>C16上传污染物分钟数据命令</summary>
    string GetSendMinuteDataCmd(DateTime dataTime, List<StatisticsData> data, int pnum = 1, int pno = 1);

    /// <summary>C17上传污染物小时数据</summary>
    Task UploadHourData(DateTime dataTime, List<StatisticsData> data, int reTryCount = 0, int timeout = -1, int pnum = 1, int pno = 1, CancellationToken cancellationToken = default);

    /// <summary>C17上传污染物小时数据(无返回变体)</summary>
    Task SendHourData(DateTime dataTime, List<StatisticsData> data, int pnum = 1, int pno = 1);

    /// <summary>C17上传污染物小时数据命令</summary>
    string GetSendHourDataCmd(DateTime dataTime, List<StatisticsData> data, int pnum = 1, int pno = 1);

    /// <summary>C18上传污染物日历史数据</summary>
    Task UploadDayData(DateTime dataTime, List<StatisticsData> data, int reTryCount = 0, int timeout = -1, int pnum = 1, int pno = 1, CancellationToken cancellationToken = default);

    /// <summary>C18上传污染物日历史数据(无返回变体)</summary>
    Task SendDayData(DateTime dataTime, List<StatisticsData> data, int pnum = 1, int pno = 1);

    /// <summary>C18上传污染物日历史数据命令</summary>
    string GetSendDayDataCmd(DateTime dataTime, List<StatisticsData> data, int pnum = 1, int pno = 1);

    /// <summary>C19上传设备运行时间日历史数据</summary>
    Task UploadRunningTimeData(DateTime dataTime, List<RunningTimeData> data, int timeout = -1);

    /// <summary>
    /// C20取污染物分钟历史数据
    /// 遵循C47-C50的规则
    /// </summary>
    event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), (List<HistoryData> HistoryDatas, bool ReturnValue, int? Timeout)> OnGetMinuteData;

    /// <summary>
    /// C21取污染物小时历史数据
    /// 遵循C47-C50的规则
    /// </summary>
    event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), (List<HistoryData> HistoryDatas, bool ReturnValue, int? Timeout)> OnGetHourData;

    /// <summary>
    /// C22取污染物日历史数据
    /// 遵循C47-C50的规则
    /// </summary>
    event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), (List<HistoryData> HistoryDatas, bool ReturnValue, int? Timeout)> OnGetDayData;

    /// <summary>
    /// C23取设备运行时间日历史数据
    /// 遵循C47-C50的规则
    /// </summary>
    event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), List<RunningTimeHistory>> OnGetRunningTimeData;

    /// <summary>C24上传数采仪开机时间</summary>
    Task UploadAcquisitionDeviceRestartTime(DateTime dataTime, DateTime restartTime, int timeout = -1);

    /// <summary>C25上传噪声声级实时数据</summary>
    Task UploadRealTimeNoiseLevel(DateTime dataTime, float noiseLevel, int timeout = -1);

    /// <summary>C26上传噪声声级分钟数据</summary>
    Task UploadMinuteNoiseLevel(DateTime dataTime, List<NoiseLevelData> data, int timeout = -1);

    /// <summary>C27上传噪声声级小时数据</summary>
    Task UploadHourNoiseLevel(DateTime dataTime, List<NoiseLevelData> data, int timeout = -1);

    /// <summary>C28上传噪声声级日历史数据</summary>
    Task UploadDayNoiseLevel(DateTime dataTime, List<NoiseLevelData_Day> data, int timeout = -1);

    /// <summary>C30零点校准量程校准</summary>
    event ActivelyPushDataEventHandler<(string PolId, RspInfo RspInfo)> OnCalibrate;

    /// <summary>C31即时采样</summary>
    event ActivelyPushDataEventHandler<(string PolId, RspInfo RspInfo)> OnRealTimeSampling;

    /// <summary>C32启动清洗/反吹</summary>
    event ActivelyPushDataEventHandler<(string PolId, RspInfo RspInfo)> OnStartCleaningOrBlowback;

    /// <summary>C33比对采样</summary>
    event ActivelyPushDataEventHandler<(string PolId, RspInfo RspInfo)> OnComparisonSampling;

    /// <summary>C34超标留样</summary>
    event ActivelyAskDataEventHandler<RspInfo, (DateTime DataTime, int VaseNo)> OnOutOfStandardRetentionSample;

    /// <summary>C35设置采样时间周期(单位：小时)</summary>
    event ActivelyPushDataEventHandler<(string PolId, TimeOnly CstartTime, int Ctime, RspInfo RspInfo)> OnSetSamplingPeriod;

    /// <summary>C36提取采样时间周期(单位：小时)</summary>
    event ActivelyAskDataEventHandler<(string PolId, RspInfo RspInfo), (TimeOnly CstartTime, int Ctime)> OnGetSamplingPeriod;

    /// <summary>C37提取出样时间(单位：分钟)</summary>
    event ActivelyAskDataEventHandler<(string PolId, RspInfo RspInfo), int> OnGetSampleExtractionTime;

    /// <summary>C38提取设备唯一标识</summary>
    event ActivelyAskDataEventHandler<(string PolId, RspInfo RspInfo), string> OnGetSN;

    /// <summary>C39上传设备唯一标识</summary>
    Task UploadSN(DateTime dataTime, string polId, string sn, int timeout = -1);

    /// <summary>C40上传现场机信息（日志）</summary>
    Task UploadLog(DateTime dataTime, string? polId, string log, int timeout = -1);

    /// <summary>C41提取现场机信息（日志）</summary>
    event ActivelyAskDataEventHandler<(string? PolId, DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), List<LogInfo>> OnGetLogInfos;

    /// <summary>
    /// C42上传现场机信息（状态）
    /// (C44上传现场机信息（参数） 同)
    /// </summary>
    Task UploadInfo(DateTime dataTime, string polId, List<DeviceInfo> deviceInfos, int timeout = -1);

    /// <summary>
    /// C43提取现场机信息（状态）
    /// (C45提取现场机信息（参数） 同)
    /// </summary>
    event ActivelyAskDataEventHandler<(string PolId, string InfoId, RspInfo RspInfo), (DateTime DataTime, List<DeviceInfo> DeviceInfos)> OnGetInfo;

    /// <summary>C46设置现场机参数</summary>
    event ActivelyPushDataEventHandler<(string PolId, string InfoId, string Info, RspInfo RspInfo)> OnSetInfo;
}
```
