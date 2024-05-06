using Communication;
using HJ212.Model;
using ProtocolInterface;

namespace HJ212
{
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
        /// (表 C.29 上传工况实时数据 同)
        /// </summary>
        Task UploadRealTimeData(DateTime dataTime, List<RealTimeData> data, int timeout = -1);

        /// <summary>C14上传污染物实时数据(无返回变体)</summary>
        Task SendRealTimeData(DateTime dataTime, List<RealTimeData> data);

        /// <summary>C15上传设备运行状态数据</summary>
        Task UploadRunningStateData(DateTime dataTime, List<RunningStateData> data, int timeout = -1);

        /// <summary>C16上传污染物分钟数据</summary>
        Task UploadMinuteData(DateTime dataTime, List<StatisticsData> data, int timeout = -1);

        /// <summary>C16上传污染物分钟数据(无返回变体)</summary>
        Task SendMinuteData(DateTime dataTime, List<StatisticsData> data);

        /// <summary>C17上传污染物小时数据</summary>
        Task UploadHourData(DateTime dataTime, List<StatisticsData> data, int timeout = -1);

        /// <summary>C17上传污染物小时数据(无返回变体)</summary>
        Task SendHourData(DateTime dataTime, List<StatisticsData> data);

        /// <summary>C18上传污染物日历史数据</summary>
        Task UploadDayData(DateTime dataTime, List<StatisticsData> data, int timeout = -1);

        /// <summary>C18上传污染物日历史数据(无返回变体)</summary>
        Task SendDayData(DateTime dataTime, List<StatisticsData> data);

        /// <summary>C19上传设备运行时间日历史数据</summary>
        Task UploadRunningTimeData(DateTime dataTime, List<RunningTimeData> data, int timeout = -1);

        /// <summary>C20取污染物分钟历史数据</summary>
        event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), (DateTime DataTime, List<StatisticsData> Data)> OnGetMinuteData;

        /// <summary>C21取污染物小时历史数据</summary>
        event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), (DateTime DataTime, List<StatisticsData> Data)> OnGetHourData;

        /// <summary>C22取污染物日历史数据</summary>
        event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), (DateTime DataTime, List<StatisticsData> Data)> OnGetDayData;

        /// <summary>C23取设备运行时间日历史数据</summary>
        event ActivelyAskDataEventHandler<(DateTime BeginTime, DateTime EndTime, RspInfo RspInfo), (DateTime DataTime, List<RunningTimeData> Data)> OnGetRunningTimeData;

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

        /// <summary>C35设置采样时间周期</summary>
        event ActivelyPushDataEventHandler<(string PolId, TimeOnly CstartTime, int Ctime, RspInfo RspInfo)> OnSetSamplingPeriod;
    }
}
