using Communication;
using HJ212.Response;
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
        Task AskSetSystemTime(string polId);

        /// <summary>C5提取实时数据间隔</summary>
        event ActivelyAskDataEventHandler<RspInfo, int> OnGetRealTimeDataInterval;

        /// <summary>C6设置实时数据间隔</summary>
        event ActivelyPushDataEventHandler<(int RtdInterval, RspInfo RspInfo)> OnSetRealTimeDataInterval;

        Task SendRealTimeData(DateTime dataTime, Dictionary<string, (string? value, string? flag)> data);

        Task SendMinuteData(DateTime dataTime, Dictionary<string, (string? avgValue, string? max, string? min, string? flag)> data);

        Task SendHourData(DateTime dataTime, Dictionary<string, (string? avgValue, string? max, string? min, string? flag)> data);

        Task SendDayData(DateTime dataTime, Dictionary<string, (string? avgValue, string? max, string? min, string? flag)> data);
    }
}
