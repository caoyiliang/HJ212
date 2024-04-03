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
        Task SendRealTimeData(DateTime dataTime, Dictionary<string, (string? value, string? flag)> data);

        Task SendMinuteData(DateTime dataTime, Dictionary<string, (string? avgValue, string? max, string? min, string? flag)> data);

        Task SendHourData(DateTime dataTime, Dictionary<string, (string? avgValue, string? max, string? min, string? flag)> data);

        Task SendDayData(DateTime dataTime, Dictionary<string, (string? avgValue, string? max, string? min, string? flag)> data);
    }
}
