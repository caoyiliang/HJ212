using Communication;
using HJ212.Response;
using ProtocolInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HJ212
{
    public interface IGB : IProtocol
    {
        /// <summary>C1设置超时时间及重发次数</summary>
        event ActivelyPushDataEventHandler<(int OverTime, int ReCount, RspInfo RspInfo)> OnSetOverTimeAndReCount;

        /// <summary>C2提取现场机时间</summary>
        event ActivelyAskDataEventHandler<(string? PolId, RspInfo RspInfo), DateTime?> OnGetSystemTime;

        Task SendRealTimeData(DateTime dataTime, Dictionary<string, (string? value, string? flag)> data);

        Task SendMinuteData(DateTime dataTime, Dictionary<string, (string? avgValue, string? max, string? min, string? flag)> data);

        Task SendHourData(DateTime dataTime, Dictionary<string, (string? avgValue, string? max, string? min, string? flag)> data);

        Task SendDayData(DateTime dataTime, Dictionary<string, (string? avgValue, string? max, string? min, string? flag)> data);
    }
}
