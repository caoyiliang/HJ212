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

        /// <summary>C14上传污染物实时数据</summary>
        Task RequestRealTimeData(DateTime dataTime, List<RealTimeData> data, int timeout = -1);

        /// <summary>C14上传污染物实时数据(无返回变体)</summary>
        Task SendRealTimeData(DateTime dataTime, List<RealTimeData> data);

        /// <summary>C15上传设备运行状态数据</summary>
        Task RequestRunningStateData(DateTime dataTime, List<RunningStateData> data, int timeout = -1);

        Task SendMinuteData(DateTime dataTime, Dictionary<string, (string? avgValue, string? max, string? min, string? flag)> data);

        Task SendHourData(DateTime dataTime, Dictionary<string, (string? avgValue, string? max, string? min, string? flag)> data);

        Task SendDayData(DateTime dataTime, Dictionary<string, (string? avgValue, string? max, string? min, string? flag)> data);
    }
}
