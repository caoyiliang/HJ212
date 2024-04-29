// See https://aka.ms/new-console-template for more information
using Communication.Bus.PhysicalPort;
using HJ212;
using HJ212.Model;

Console.WriteLine("Hello, World!");
IGB gb = new GB("通道一", new SerialPort(), "88888888");

gb.OnSetOverTimeAndReCount += Gb_OnSetOverTimeAndReCount;
gb.OnGetSystemTime += Gb_OnGetSystemTime;
gb.OnSetSystemTime += Gb_OnSetSystemTime;
gb.OnGetRealTimeDataInterval += Gb_OnGetRealTimeDataInterval;
gb.OnSetRealTimeDataInterval += Gb_OnSetRealTimeDataInterval;
gb.OnGetMinuteDataInterval += Gb_OnGetMinuteDataInterval;
gb.OnSetMinuteDataInterval += Gb_OnSetMinuteDataInterval;
gb.OnSetNewPW += Gb_OnSetNewPW;
gb.OnStartRealTimeData += Gb_OnStartRealTimeData;
gb.OnStopRealTimeData += Gb_OnStopRealTimeData;
gb.OnStartRunningStateData += Gb_OnStartRunningStateData;
gb.OnStopRunningStateData += Gb_OnStopRunningStateData;

//测试 QN=20160801085857223;ST=32;CN=2022;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&&&
async Task Gb_OnStopRunningStateData(RspInfo objects)
{
    await Task.CompletedTask;
}

//测试 QN=20160801085857223;ST=32;CN=2021;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&&&
async Task Gb_OnStartRunningStateData(RspInfo objects)
{
    await Task.CompletedTask;
}

//测试 QN=20160801085857223;ST=32;CN=2012;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&&&
async Task Gb_OnStopRealTimeData(RspInfo objects)
{
    await Task.CompletedTask;
}

//测试 QN=20160801085857223;ST=32;CN=2011;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&&&
async Task Gb_OnStartRealTimeData(RspInfo objects)
{
    await Task.CompletedTask;
}

//测试 QN=20160801085857223;ST=32;CN=1072;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&NewPW=654321&&
async Task Gb_OnSetNewPW((string NewPW, RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

//测试 QN=20160801085857223;ST=32;CN=1064;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&MinInterval=10&&s
async Task Gb_OnSetMinuteDataInterval((int MinInterval, RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

//测试 QN=20160801085857223;ST=32;CN=1063;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&&&
Task<int> Gb_OnGetMinuteDataInterval(RspInfo objects)
{
    return Task.FromResult(10);
}

//测试 QN=20160801085857223;ST=32;CN=1062;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&RtdInterval=30&&
Task Gb_OnSetRealTimeDataInterval((int RtdInterval, RspInfo RspInfo) objects)
{
    return Task.CompletedTask;
}

//测试 QN=20160801085857223;ST=32;CN=1061;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&&&
Task<int> Gb_OnGetRealTimeDataInterval(RspInfo objects)
{
    return Task.FromResult(5);
}

//测试 QN=20160801085857223;ST=32;CN=1012;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&PolId=w01018;SystemTime=20160801085857&&
async Task Gb_OnSetSystemTime((string? PolId, DateTime SystemTime, RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

//测试 QN=20160801085857223;ST=32;CN=1011;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&PolId=w01018&&
async Task<DateTime?> Gb_OnGetSystemTime((string? PolId, RspInfo RspInfo) objects)
{
    //回复空，则取系统时间回复
    return await Task.FromResult<DateTime?>(default);
}

//测试 QN=20160801085857223;ST=32;CN=1000;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&OverTime=5;ReCount=3&&
async Task Gb_OnSetOverTimeAndReCount((int OverTime, int ReCount, RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

await gb.OpenAsync();

try
{
    //测试 QN=20160801085857223;ST=91;CN=9013;PW=123456;MN=010000A8900016F000169DC0;Flag=4;CP=&& &&
    await gb.AskSetSystemTime("a1001");
}
catch (TimeoutException) { }

List<RealTimeData> realTimeDatas =
[
    new RealTimeData("a1001", "50.4"){Flag="N"},
    new RealTimeData("a1002", "35.2"){Flag="D"}
];
try
{
    //测试 QN=20240429103553533;ST=91;CN=9014;PW=123456;MN=010000A8900016F000169DC0;Flag=4;CP=&&&&
    await gb.RequestRealTimeData(DateTime.Now, realTimeDatas);
}
catch (TimeoutException) { }

await gb.SendRealTimeData(DateTime.Now, realTimeDatas);

try
{
    //测试 QN=20240429114224393;ST=91;CN=9014;PW=123456;MN=010000A8900016F000169DC0;Flag=4;CP=&&&&
    await gb.RequestRunningStateData(DateTime.Now, [new RunningStateData("SB1", "1"), new RunningStateData("SB2", "2")]);
}
catch (TimeoutException) { }

await gb.SendMinuteData(DateTime.Now, new Dictionary<string, (string? avgValue, string? max, string? min, string? flag)>() { { "a1001", ("50.4", "60.5", "30.7", "N") }, { "a1002", ("35.2", "50.1", "10.2", "D") } });
await gb.SendHourData(DateTime.Now, new Dictionary<string, (string? avgValue, string? max, string? min, string? flag)>() { { "a1001", ("50.4", "60.5", "30.7", "N") }, { "a1002", ("35.2", "50.1", "10.2", "D") } });
await gb.SendDayData(DateTime.Now, new Dictionary<string, (string? avgValue, string? max, string? min, string? flag)>() { { "a1001", ("50.4", "60.5", "30.7", "N") }, { "a1002", ("35.2", "50.1", "10.2", "D") } });

Console.ReadLine();