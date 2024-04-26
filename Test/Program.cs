// See https://aka.ms/new-console-template for more information
using Communication.Bus.PhysicalPort;
using HJ212;

Console.WriteLine("Hello, World!");
IGB gb = new GB("通道一", new SerialPort(), "88888888");

gb.OnSetOverTimeAndReCount += Gb_OnSetOverTimeAndReCount;

async Task Gb_OnSetOverTimeAndReCount((int OverTime, int ReCount, HJ212.Response.RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

await gb.OpenAsync();

await gb.SendRealTimeData(DateTime.Now, new Dictionary<string, (string? value, string? flag)>() { { "a1001", ("50.4", "N") }, { "a1002", ("35.2", "D") } });
await gb.SendMinuteData(DateTime.Now, new Dictionary<string, (string? avgValue, string? max, string? min, string? flag)>() { { "a1001", ("50.4", "60.5", "30.7", "N") }, { "a1002", ("35.2", "50.1", "10.2", "D") } });
await gb.SendHourData(DateTime.Now, new Dictionary<string, (string? avgValue, string? max, string? min, string? flag)>() { { "a1001", ("50.4", "60.5", "30.7", "N") }, { "a1002", ("35.2", "50.1", "10.2", "D") } });
await gb.SendDayData(DateTime.Now, new Dictionary<string, (string? avgValue, string? max, string? min, string? flag)>() { { "a1001", ("50.4", "60.5", "30.7", "N") }, { "a1002", ("35.2", "50.1", "10.2", "D") } });

Console.ReadLine();