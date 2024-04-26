// See https://aka.ms/new-console-template for more information
using Communication.Bus.PhysicalPort;
using HJ212;

Console.WriteLine("Hello, World!");
IGB gb = new GB("通道一", new SerialPort(), "88888888");

gb.OnSetOverTimeAndReCount += Gb_OnSetOverTimeAndReCount;
gb.OnGetSystemTime += Gb_OnGetSystemTime;
gb.OnSetSystemTime += Gb_OnSetSystemTime;

//测试 
async Task Gb_OnSetSystemTime((string? PolId, DateTime SystemTime, HJ212.Response.RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

//测试 QN=20160801085857223;ST=32;CN=1011;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&PolId=w01018&&
async Task<DateTime?> Gb_OnGetSystemTime((string? PolId, HJ212.Response.RspInfo RspInfo) objects)
{
    //回复空，则取系统时间回复
    return await Task.FromResult<DateTime?>(default);
}

//测试 QN=20160801085857223;ST=32;CN=1000;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&OverTime=5;ReCount=3&&
async Task Gb_OnSetOverTimeAndReCount((int OverTime, int ReCount, HJ212.Response.RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

await gb.OpenAsync();

await gb.AskSetSystemTime("a1001");

await gb.SendRealTimeData(DateTime.Now, new Dictionary<string, (string? value, string? flag)>() { { "a1001", ("50.4", "N") }, { "a1002", ("35.2", "D") } });
await gb.SendMinuteData(DateTime.Now, new Dictionary<string, (string? avgValue, string? max, string? min, string? flag)>() { { "a1001", ("50.4", "60.5", "30.7", "N") }, { "a1002", ("35.2", "50.1", "10.2", "D") } });
await gb.SendHourData(DateTime.Now, new Dictionary<string, (string? avgValue, string? max, string? min, string? flag)>() { { "a1001", ("50.4", "60.5", "30.7", "N") }, { "a1002", ("35.2", "50.1", "10.2", "D") } });
await gb.SendDayData(DateTime.Now, new Dictionary<string, (string? avgValue, string? max, string? min, string? flag)>() { { "a1001", ("50.4", "60.5", "30.7", "N") }, { "a1002", ("35.2", "50.1", "10.2", "D") } });

Console.ReadLine();