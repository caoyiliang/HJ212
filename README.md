# HJ212国标协议：

``` C#
IGB gb = new GB("输出一", new SerialPort(), "88888888", version: HJ212.Version.HJT212_2025);
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

# 实现HJ212-2017/2025《污染源在线自动监控（监测）系统数据传输标准》中 附 录 C（资料性附录）通讯命令示例和拆分包及应答机制示例 中所有（表1-表50）详细接口如下，若有错误和建议请及时联系我改正：

``` C#
/// <summary>
/// 2025:C1
/// 2017:C1
/// 设置超时时间及重发次数
/// </summary>
event ActivelyPushDataEventHandler<(int OverTime, int ReCount, RspInfo RspInfo)> OnSetOverTimeAndReCount;

/// <summary>
/// 2025:C2
/// 上传数采仪硬件序号
/// </summary>
Task UploadHardwareSN(string dataLoggerId, string cpuId, string mac1, string mac2, int timeout = -1);

/// <summary>
/// 2025:C3
/// 设置数采仪MN编码
/// </summary>
event ActivelyPushDataEventHandler<(string? DataLoggerId, RspInfo RspInfo)> OnSetMN;

/// <summary>
/// 2025:C4
/// 2017:C2
/// 提取现场机时间
/// </summary>
event ActivelyAskDataEventHandler<(string? PolId, RspInfo RspInfo), DateTime?> OnGetSystemTime;

/// <summary>
/// 2025:C5
/// 2017:C3
/// 设置现场机时间
/// </summary>
event ActivelyPushDataEventHandler<(string? PolId, DateTime SystemTime, RspInfo RspInfo)> OnSetSystemTime;

/// <summary>
/// 2025:C6
/// 2017:C4
/// 现场机时间校准请求
/// </summary>
Task AskSetSystemTime(string polId, int timeout = -1);

/// <summary>
/// 2025:C7
/// 现场机获取新密钥
/// </summary>
Task AskNewSK(string? mSKCreateTime = null, int timeout = -1);

/// <summary>
/// 2025:C8
/// 上位机设置新密钥
/// </summary>
event ActivelyPushDataEventHandler<(string SKCreateTime, string NewSK, RspInfo RspInfo)> OnSetNewSK;

/// <summary>
/// 2025:C9
/// 2017:C5
/// 提取实时数据间隔
/// </summary>
event ActivelyAskDataEventHandler<RspInfo, int> OnGetRealTimeDataInterval;

/// <summary>
/// 2025:C10
/// 2017:C6
/// 设置实时数据间隔
/// </summary>
event ActivelyPushDataEventHandler<(int RtdInterval, RspInfo RspInfo)> OnSetRealTimeDataInterval;

/// <summary>
/// 2025:C11
/// 2017:C7
/// 提取分钟数据间隔
/// </summary>
event ActivelyAskDataEventHandler<RspInfo, int> OnGetMinuteDataInterval;

/// <summary>
/// 2025:C12
/// 2017:C8
/// 设置分钟数据间隔
/// </summary>
event ActivelyPushDataEventHandler<(int MinInterval, RspInfo RspInfo)> OnSetMinuteDataInterval;

/// <summary>
/// 2025:C13
/// 2017:C9
/// 设置现场机访问密码
/// </summary>
event ActivelyPushDataEventHandler<(string NewPW, RspInfo RspInfo)> OnSetNewPW;

/// <summary>
/// 2025:C14
/// 2017:C10
/// 取污染物实时数据
/// </summary>
event ActivelyPushDataEventHandler<RspInfo> OnStartRealTimeData;

/// <summary>
/// 2025:C15
/// 2017:C11
/// 停止察看污染物实时数据
/// </summary>
event ActivelyPushDataEventHandler<RspInfo> OnStopRealTimeData;

/// <summary>
/// 2025:C16
/// 2017:C12
/// 取设备运行状态数据
/// </summary>
event ActivelyPushDataEventHandler<RspInfo> OnStartRunningStateData;

/// <summary>
/// 2025:C17
/// 2017:C13
/// 停止察看设备运行状态
/// </summary>
event ActivelyPushDataEventHandler<RspInfo> OnStopRunningStateData;

/// <summary>
/// 2025:C18
/// 2017:C14 (C29上传工况实时数据 同)
/// 上传污染物实时数据
/// </summary>
/// <param name="dataTime">数据时间，表示一个时间点，时间精确到秒；20240601085857表示上传数据为2024年6月1日8时58分57秒的监测参数实时数据</param>
/// <param name="data">实时数据包集</param>
/// <param name="timeout">超时时间，单位毫秒，默认-1表示使用构造中的超时时间</param>
Task UploadRealTimeData(DateTime dataTime, List<RealTimeData> data, int timeout = -1);

/// <summary>
/// 2025:C18
/// 2017:C14 (C29上传工况实时数据 同)
/// 上传污染物实时数据(无返回变体)
/// </summary>
/// <param name="dataTime">数据时间，表示一个时间点，时间精确到秒；20240601085857表示上传数据为2024年6月1日8时58分57秒的监测参数实时数据</param>
/// <param name="data">实时数据包集</param>
Task SendRealTimeData(DateTime dataTime, List<RealTimeData> data);

/// <summary>
/// 2025:C19
/// 2017:C15
/// 上传设备运行状态数据
/// </summary>
/// <param name="dataTime">数据时间，表示一个时间点，时间精确到秒；20240601085857表示上传数据为2024年6月1日8时58分57秒的污染治理设施运行状态</param>
/// <param name="data">运行状态数据</param>
/// <param name="timeout">超时时间，单位毫秒，默认-1表示使用构造中的超时时间</param>
Task UploadRunningStateData(DateTime dataTime, List<RunningStateData> data, int timeout = -1);

/// <summary>
/// 2025:C20
/// 2017:C16
/// 上传污染物分钟数据
/// </summary>
/// <param name="dataTime">数据时间，表示一个时间段的开始时间点，时间精确到分钟；若分钟数据上传时间间隔取值为1 min，则20240601080100表示上传数据为时间段2024年6月1日8时01分0秒到2024年6月1日8时02分0秒之间的监测参数分钟数据</param>
/// <param name="data">分钟数据包集</param>
/// <param name="reTryCount">重发次数</param>
/// <param name="timeout">超时时间，单位毫秒，默认-1表示使用构造中的超时时间</param>
/// <param name="pnum">总包数</param>
/// <param name="pno">当前包号</param>
/// <param name="cancellationToken">取消令牌</param>
Task UploadMinuteData(DateTime dataTime, List<StatisticsData> data, int reTryCount = 0, int timeout = -1, int pnum = 1, int pno = 1, CancellationToken cancellationToken = default);

/// <summary>
/// 2025:C20
/// 2017:C16
/// 上传污染物分钟数据(无返回变体)
/// </summary>
/// <param name="dataTime">数据时间，表示一个时间段的开始时间点，时间精确到分钟；若分钟数据上传时间间隔取值为1 min，则20240601080100表示上传数据为时间段2024年6月1日8时01分0秒到2024年6月1日8时02分0秒之间的监测参数分钟数据</param>
/// <param name="data">分钟数据包集</param>
/// <param name="pnum">总包数</param>
/// <param name="pno">当前包号</param>
Task SendMinuteData(DateTime dataTime, List<StatisticsData> data, int pnum = 1, int pno = 1);

/// <summary>
/// 2025:C20
/// 2017:C16
/// 上传污染物分钟数据命令
/// </summary>
/// <param name="dataTime">数据时间，表示一个时间段的开始时间点，时间精确到分钟；若分钟数据上传时间间隔取值为1 min，则20240601080100表示上传数据为时间段2024年6月1日8时01分0秒到2024年6月1日8时02分0秒之间的监测参数分钟数据</param>
/// <param name="data">分钟数据包集</param>
/// <param name="pnum">总包数</param>
/// <param name="pno">当前包号</param>
string GetSendMinuteDataCmd(DateTime dataTime, List<StatisticsData> data, int pnum = 1, int pno = 1);

/// <summary>
/// 2025:C21
/// 2017:C17
/// 上传污染物小时数据
/// </summary>
/// <param name="dataTime">数据时间，表示一个时间段的开始时间点，时间精确到小时； 20240601080000表示上传数据为时间段2024年6月1日8时0分0秒到2024年6月1日9时0分0秒之间的监测参数小时数据</param>
/// <param name="data">小时数据包集</param>
/// <param name="reTryCount">重发次数</param>
/// <param name="timeout">超时时间，单位毫秒，默认-1表示使用构造中的超时时间</param>
/// <param name="pnum">总包数</param>
/// <param name="pno">当前包号</param>
/// <param name="cancellationToken">取消令牌</param>
Task UploadHourData(DateTime dataTime, List<StatisticsData> data, int reTryCount = 0, int timeout = -1, int pnum = 1, int pno = 1, CancellationToken cancellationToken = default);

/// <summary>
/// 2025:C21
/// 2017:C17
/// 上传污染物小时数据(无返回变体)
/// </summary>
/// <param name="dataTime">数据时间，表示一个时间段的开始时间点，时间精确到小时； 20240601080000表示上传数据为时间段2024年6月1日8时0分0秒到2024年6月1日9时0分0秒之间的监测参数小时数据</param>
/// <param name="data">小时数据包集</param>
/// <param name="pnum">总包数</param>
/// <param name="pno">当前包号</param>
Task SendHourData(DateTime dataTime, List<StatisticsData> data, int pnum = 1, int pno = 1);

/// <summary>
/// 2025:C21
/// 2017:C17
/// 上传污染物小时数据命令
/// </summary>
/// <param name="dataTime">数据时间，表示一个时间段的开始时间点，时间精确到小时； 20240601080000表示上传数据为时间段2024年6月1日8时0分0秒到2024年6月1日9时0分0秒之间的监测参数小时数据</param>
/// <param name="data">小时数据包集</param>
/// <param name="pnum">总包数</param>
/// <param name="pno">当前包号</param>
string GetSendHourDataCmd(DateTime dataTime, List<StatisticsData> data, int pnum = 1, int pno = 1);

/// <summary>
/// 2025:C22
/// 2017:C18
/// 上传污染物日历史数据
/// </summary>
/// <param name="dataTime">数据时间，表示一个时间段的开始时间点，时间精确到日；“ 20240601000000”表示上传数据为时间段2024年6月1日0时0分0秒到2024年6月2日0时0分0秒之间的日数据</param>
/// <param name="data">日数据包集</param>
/// <param name="reTryCount">重发次数</param>
/// <param name="timeout">超时时间，单位毫秒，默认-1表示使用构造中的超时时间</param>
/// <param name="pnum">总包数</param>
/// <param name="pno">当前包号</param>
/// <param name="cancellationToken">取消令牌</param>
Task UploadDayData(DateTime dataTime, List<StatisticsData> data, int reTryCount = 0, int timeout = -1, int pnum = 1, int pno = 1, CancellationToken cancellationToken = default);

/// <summary>
/// 2025:C22
/// 2017:C18
/// 上传污染物日历史数据(无返回变体)
/// </summary>
/// <param name="dataTime">数据时间，表示一个时间段的开始时间点，时间精确到日；“ 20240601000000”表示上传数据为时间段2024年6月1日0时0分0秒到2024年6月2日0时0分0秒之间的日数据</param>
/// <param name="data">日数据包集</param>
/// <param name="pnum">总包数</param>
/// <param name="pno">当前包号</param>
Task SendDayData(DateTime dataTime, List<StatisticsData> data, int pnum = 1, int pno = 1);

/// <summary>
/// 2025:C22
/// 2017:C18
/// 上传污染物日历史数据命令
/// </summary>
/// <param name="dataTime">数据时间，表示一个时间段的开始时间点，时间精确到日；“ 20240601000000”表示上传数据为时间段2024年6月1日0时0分0秒到2024年6月2日0时0分0秒之间的日数据</param>
/// <param name="data">日数据包集</param>
/// <param name="pnum">总包数</param>
/// <param name="pno">当前包号</param>
string GetSendDayDataCmd(DateTime dataTime, List<StatisticsData> data, int pnum = 1, int pno = 1);

/// <summary>
/// 2025:C23
/// 2017:C19
/// 上传设备运行时间日历史数据
/// </summary>
Task UploadRunningTimeData(DateTime dataTime, List<RunningTimeData> data, int timeout = -1);

/// <summary>
/// 2025:C24
/// 2017:C20 遵循C47-C50的规则
/// 取污染物分钟历史数据
/// </summary>
event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), (List<HistoryData> HistoryDatas, bool ReturnValue, int? Timeout)> OnGetMinuteData;

/// <summary>
/// 2025:C25
/// 2017:C21 遵循C47-C50的规则
/// 取污染物小时历史数据
/// </summary>
event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), (List<HistoryData> HistoryDatas, bool ReturnValue, int? Timeout)> OnGetHourData;

/// <summary>
/// 2025:C26
/// 2017:C22 遵循C47-C50的规则
/// 取污染物日历史数据
/// </summary>
event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), (List<HistoryData> HistoryDatas, bool ReturnValue, int? Timeout)> OnGetDayData;

/// <summary>
/// 2025:C27
/// 取原始监测数据（周期性监测）
/// </summary>
event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), List<HistoryRawData>> OnGetRawData;

/// <summary>
/// 2025:C28
/// 上传原始监测数据（周期性监测）
/// </summary>
Task UploadRawData(DateTime dataTime, List<RawData> data, int timeout = -1);

/// <summary>
/// 2025:C29
/// 2017:C23 遵循C47-C50的规则
/// 取设备运行时间日历史数据
/// </summary>
event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), List<RunningTimeHistory>> OnGetRunningTimeData;

/// <summary>
/// 2025:C30
/// 2017:C24
/// 上传数采仪开机时间
/// </summary>
Task UploadAcquisitionDeviceRestartTime(DateTime dataTime, DateTime restartTime, int timeout = -1);

/// <summary>
/// 2025:C31
/// 上传炉膛温度5 min均值
/// </summary>
/// <param name="dataTime">数据时间，表示5min数据的时间区间，时间精确到分钟；“20240601011000”表示2024年6月1日01点10分至15分之间（不含15分）的炉膛温度5min平均值。</param>
/// <param name="avgData">炉膛温度5min平均值</param>
/// <param name="timeout">超时时间，单位毫秒，默认-1表示使用构造中的超时时间</param>
Task UploadFurnaceTemperature(DateTime dataTime, string avgData, int timeout = -1);

/// <summary>
/// 2025:C32
/// 2017:C25
/// 上传噪声声级实时数据
/// </summary>
/// <param name="dataTime">数据时间，表示一个时间点，时间精确到秒；20240601085857表示上传数据为2024年6月1日8时58分57秒的噪声声级实时数据</param>
/// <param name="noiseLevel">噪声瞬时声级</param>
/// <param name="timeout">超时时间，单位毫秒，默认-1表示使用构造中的超时时间</param>
Task UploadRealTimeNoiseLevel(DateTime dataTime, float noiseLevel, int timeout = -1);

/// <summary>
/// 2025:C33
/// 2017:C26
/// 上传噪声声级分钟数据
/// </summary>
/// <param name="dataTime">数据时间，表示一个时间段的开始时间点，时间精确到分钟；噪声声级分钟数据上传时间间隔取值为1 min，“20240601084000”表示上传数据为时间段2024年6月1日8时40分0秒到2024年6月1日8时41分0秒之间的噪声分钟数据</param>
/// <param name="data">分钟数据时间间隔内L5值</param>
/// <param name="timeout">超时时间，单位毫秒，默认-1表示使用构造中的超时时间</param>
Task UploadMinuteNoiseLevel(DateTime dataTime, List<NoiseLevelData> data, int timeout = -1);

/// <summary>
/// 2017:C27
/// 上传噪声声级小时数据
/// </summary>
Task UploadHourNoiseLevel(DateTime dataTime, List<NoiseLevelData> data, int timeout = -1);

/// <summary>
/// 2025:C34
/// 上传噪声声级单次测量数据
/// </summary>
/// <param name="dataTime">数据时间，表示单次测量的开始时间点，时间精确到分钟；若单次测量周期为20分钟，则“20240601080000”表示2024年6月1日8点0分0秒至2024年6月1日8点20分0秒的噪声数据。</param>
/// <param name="data">单次测量时间段内值</param>
/// <param name="timeout">超时时间，单位毫秒，默认-1表示使用构造中的超时时间</param>
Task UploadSingleNoiseLevel(DateTime dataTime, List<NoiseLevelData> data, int timeout = -1);

/// <summary>
/// 2025:C35
/// 2017:C28
/// 上传噪声声级日历史数据
/// </summary>
/// <param name="dataTime">数据时间，表示一个时间段的开始时间点，时间精确到日；“20240601000000”表示上传数据为时间段2024年6月1日0时0分0秒到2024年6月2日0时0分0秒之间的噪声声级日数据</param>
/// <param name="data">日数据</param>
/// <param name="timeout">超时时间，单位毫秒，默认-1表示使用构造中的超时时间</param>
Task UploadDayNoiseLevel(DateTime dataTime, List<NoiseLevelData_Day> data, int timeout = -1);

/// <summary>
/// 2025:C36
/// 上传关键生产工况实时数据
/// </summary>
/// <param name="dataTime">数据时间，表示一个时间点，时间精确到秒；“20240601084500”表示上传数据为2024 年6月1日8时45分00秒的工况实时数据</param>
/// <param name="data">关键生产工况实时数据</param>
/// <param name="timeout">超时时间，单位毫秒，默认-1表示使用构造中的超时时间</param>
Task UploadKeyProductionConditionRealTimeData(DateTime dataTime, List<KeyProductionConditionData> data, int timeout = -1);

/// <summary>
/// 2025:C37
/// 上传生产设施的用电监测实时数据
/// </summary>
/// <param name="dataTime">数据时间，表示一个时间点，时间精确到秒；“20240601084500”表示上传数据为2024年6月1日8时45分00秒的实时数据</param>
/// <param name="data">生产设施的用电监测实时数据</param>
/// <param name="timeout">超时时间，单位毫秒，默认-1表示使用构造中的超时时间</param>
Task UploadElectricityMonitoringRealTimeDataForProduction(DateTime dataTime, List<ElectricityMonitoringData> data, int timeout = -1);

///<summary>
///2025:C38
///上传治理设施的用电监测实时数据
///</summary>
Task UploadElectricityMonitoringRealTimeDataForTreatment(DateTime dataTime, List<ElectricityMonitoringData> data, int timeout = -1);

/// <summary>
/// 2025:C39、C40
/// 取生产设施用电监测历史数据（实时）
/// 取治理设施用电监测历史数据（实时）
/// </summary>
event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), List<ElectricityMonitoringHistory>> OnGetElectricityMonitoringRealTimeData;

/// <summary>
/// 2025:C41
/// 2017:C30
/// 零点（量程）校准与调整
/// </summary>
event ActivelyPushDataEventHandler<(string PolId, CalibrationType? CalibrationType, RspInfo RspInfo)> OnCalibrate;

/// <summary>
/// 2025:C42
/// 2017:C31
/// 即时采样
/// </summary>
event ActivelyPushDataEventHandler<(string PolId, RspInfo RspInfo)> OnRealTimeSampling;

/// <summary>
/// 2025:C43
/// 2017:C32
/// 启动清洗/反吹
/// </summary>
event ActivelyPushDataEventHandler<(string PolId, RspInfo RspInfo)> OnStartCleaningOrBlowback;

/// <summary>
/// 2017:C33
/// 比对采样
/// </summary>
event ActivelyPushDataEventHandler<(string PolId, RspInfo RspInfo)> OnComparisonSampling;

/// <summary>
/// 2017:C34
/// 超标留样
/// </summary>
event ActivelyAskDataEventHandler<RspInfo, (DateTime DataTime, int VaseNo)> OnOutOfStandardRetentionSample;

/// <summary>
/// 2017:C35
/// 设置采样时间周期(单位：小时)
/// </summary>
event ActivelyPushDataEventHandler<(string PolId, TimeOnly CstartTime, int Ctime, RspInfo RspInfo)> OnSetSamplingPeriod;

/// <summary>
/// 2017:C36
/// 提取采样时间周期(单位：小时)
/// </summary>
event ActivelyAskDataEventHandler<(string PolId, RspInfo RspInfo), (TimeOnly CstartTime, int Ctime)> OnGetSamplingPeriod;

/// <summary>
/// 2025:C44
/// 2017:C37
/// 提取出样时间(单位：分钟)
/// </summary>
event ActivelyAskDataEventHandler<(string PolId, RspInfo RspInfo), string> OnGetSampleExtractionTime;

/// <summary>
/// 2025:C45
/// 2017:C38
/// 提取设备唯一标识
/// </summary>
event ActivelyAskDataEventHandler<(string PolId, RspInfo RspInfo), string> OnGetSN;

/// <summary>
/// 2025:C46
/// 2017:C39
/// 上传设备唯一标识
/// </summary>
Task UploadSN(DateTime dataTime, string polId, string sn, int timeout = -1);

/// <summary>
/// 2025:C47
/// 2017:C40
/// 上传现场机信息（日志）
/// </summary>
Task UploadLog(DateTime dataTime, string? polId, string log, int timeout = -1);

/// <summary>
/// 2025:C48
/// 2017:C41
/// 提取现场机信息（日志）
/// </summary>
event ActivelyAskDataEventHandler<(string? PolId, DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), List<LogInfo>> OnGetLogInfos;

/// <summary>
/// 2025:C49 (C51上传现场机信息（参数） 同)
/// 2017:C42 (C44上传现场机信息（参数） 同)
/// 上传现场机信息（状态）
/// </summary>
/// <param name="dataTime">数据时间，表示一个时间点，时间精确到秒；“20240601085857”表示2024年6月1日8时58分57秒的状态</param>
/// <param name="polId">对应污染物编码</param>
/// <param name="deviceInfos">现场机信息列表</param>
/// <param name="timeout">超时时间，单位毫秒，默认-1表示使用构造中的超时时间</param>
Task UploadInfo(DateTime dataTime, string polId, List<DeviceInfo> deviceInfos, int timeout = -1);

/// <summary>
/// 2025:C50 (C52提取现场机信息（参数） 同)
/// 2017:C43 (C45提取现场机信息（参数） 同)
/// 提取现场机信息（状态）
/// </summary>
event ActivelyAskDataEventHandler<(string PolId, string InfoId, RspInfo RspInfo), (DateTime DataTime, List<DeviceInfo> DeviceInfos)> OnGetInfo;

/// <summary>
/// 2025:C53
/// 2017:C46
/// 设置现场机参数
/// </summary>
event ActivelyPushDataEventHandler<(string PolId, string InfoId, string Info, RspInfo RspInfo)> OnSetInfo;

/// <summary>
/// 2025:C54
/// 现场机发送心跳包
/// </summary>
/// <param name="returnValue">是否需要服务端回复</param>
/// <param name="timeout">超时时间，单位毫秒，默认-1表示使用构造中的超时时间</param>
Task SendHeartbeat(bool returnValue = true, int timeout = -1);

/// <summary>
/// 2025:C59
/// 上传自动标样核查（校准）数据
/// </summary>
/// <param name="dataTime">自动标样核查启动时间（自动校准完成使用标准溶液验证开始时间）</param>
/// <param name="polId">对应污染物编码</param>
/// <param name="sampleRd">标准溶液实际测量浓度示值</param>
/// <param name="resultType">自动标样核查结果或校准后验证结果类型，1表示通过，0表示未通过</param>
/// <param name="standardValue">自动标样核查（校准）标准溶液浓度标称值</param>
/// <param name="timeout">超时时间，单位毫秒，默认-1表示使用构造中的超时时间</param>
/// <returns></returns>
Task UploadAutoStandardCheckData(DateTime dataTime, string polId, string sampleRd, string resultType, string standardValue, int timeout = -1);

/// <summary>
/// 2025:C60
/// 提取自动标样核查（校准）数据
/// </summary>
event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), List<AutoStandardCheckData>> OnGetAutoStandardCheckData;

/// <summary>
/// 2025:C61
/// 启动自动标样核查
/// </summary>
event ActivelyPushDataEventHandler<(string PolId, RspInfo RspInfo)> OnStartAutoStandardCheck;

/// <summary>
/// 2025:C62
/// 下发即时留样任务
/// </summary>
event ActivelyPushDataEventHandler<(string PolId, RspInfo RspInfo)> OnStartRealTimeSampleTask;

/// <summary>
/// 2025:C63
/// 上传留样信息
/// </summary>
Task UploadSampleInfo(DateTime dataTime, string vaseNo, List<SampleInfo> sampleInfos, int timeout = -1);

/// <summary>
/// 补发统计数据
/// </summary>
Task ReissueStatisticsData(string data, int reTryCount = 0, int timeout = -1, CancellationToken cancellationToken = default);
```
