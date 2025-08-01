﻿// See https://aka.ms/new-console-template for more information
using Communication.Bus.PhysicalPort;
using HJ212;
using HJ212.Model;

Console.WriteLine("Hello, World!");
var gb = new GB("通道一", new TcpClient("127.0.0.1", 2756), "88888888", version: HJ212.Version.HJT212_2025);

gb.OnSetOverTimeAndReCount += Gb_OnSetOverTimeAndReCount;
gb.OnSetMN += Gb_OnSetMN;
gb.OnGetSystemTime += Gb_OnGetSystemTime;
gb.OnSetSystemTime += Gb_OnSetSystemTime;
gb.OnSetNewSK += Gb_OnSetNewSK;
gb.OnGetRealTimeDataInterval += Gb_OnGetRealTimeDataInterval;
gb.OnSetRealTimeDataInterval += Gb_OnSetRealTimeDataInterval;
gb.OnGetMinuteDataInterval += Gb_OnGetMinuteDataInterval;
gb.OnSetMinuteDataInterval += Gb_OnSetMinuteDataInterval;
gb.OnSetNewPW += Gb_OnSetNewPW;
gb.OnStartRealTimeData += Gb_OnStartRealTimeData;
gb.OnStopRealTimeData += Gb_OnStopRealTimeData;
gb.OnStartRunningStateData += Gb_OnStartRunningStateData;
gb.OnStopRunningStateData += Gb_OnStopRunningStateData;
gb.OnGetMinuteData += Gb_OnGetMinuteData;
gb.OnGetHourData += Gb_OnGetHourData;
gb.OnGetDayData += Gb_OnGetDayData;
gb.OnGetRawData += Gb_OnGetRawData;
gb.OnGetRunningTimeData += Gb_OnGetRunningTimeData;
gb.OnGetElectricityMonitoringRealTimeData += Gb_OnGetElectricityMonitoringRealTimeData;
gb.OnCalibrate += Gb_OnCalibrate;
gb.OnRealTimeSampling += Gb_OnRealTimeSampling;
gb.OnStartCleaningOrBlowback += Gb_OnStartCleaningOrBlowback;
gb.OnComparisonSampling += Gb_OnComparisonSampling;
gb.OnOutOfStandardRetentionSample += Gb_OnOutOfStandardRetentionSample;
gb.OnSetSamplingPeriod += Gb_OnSetSamplingPeriod;
gb.OnGetSamplingPeriod += Gb_OnGetSamplingPeriod;
gb.OnGetSampleExtractionTime += Gb_OnGetSampleExtractionTime;
gb.OnGetSN += Gb_OnGetSN;
gb.OnGetLogInfos += Gb_OnGetLogInfos;
gb.OnGetInfo += Gb_OnGetInfo;
gb.OnSetInfo += Gb_OnSetInfo;
gb.OnGetAutoStandardCheckData += Gb_OnGetAutoStandardCheckData;
gb.OnStartAutoStandardCheck += Gb_OnStartAutoStandardCheck;
gb.OnStartRealTimeSampleTask += Gb_OnStartRealTimeSampleTask;

//测试 QN=20240601085857223;ST=32;CN=3022;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&PolId=w01018&&
async Task Gb_OnStartRealTimeSampleTask((string PolId, RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

//测试 QN=20240601085857223;ST=32;CN=3024;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&PolId=w01018&&
async Task Gb_OnStartAutoStandardCheck((string PolId, RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

//测试 QN=20240601085857223;ST=32;CN=2062;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&BeginTime=20240601080000;EndTime=20240601080000&& 
Task<List<AutoStandardCheckData>> Gb_OnGetAutoStandardCheckData((DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) objects)
{
    return Task.FromResult(new List<AutoStandardCheckData>
    {
        new(DateTime.Now,"a1001"){ SampleRd = "15.3", ResultType = "1", StandardValue = "12"},
        new(DateTime.Now.AddHours(-1),"a1002"){ SampleRd = "20.5", ResultType = "0", StandardValue = "18"}
    });
}

//测试 QN=20240601085857223;ST=32;CN=3021;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&PolId=w01018;InfoId=i13004;i13004-Info=168.0&& 
async Task Gb_OnSetInfo((string PolId, string InfoId, string Info, RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

//测试 QN=20240601085857223;ST=32;CN=3020;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&PolId=w01018;InfoId=i12001&&
async Task<(DateTime DataTime, List<DeviceInfo> DeviceInfos)> Gb_OnGetInfo((string PolId, string InfoId, RspInfo RspInfo) objects)
{
    return await Task.FromResult((DateTime.Now, new List<DeviceInfo> { new("i12001", "1"), new("i12002", "1") }));
}
;

//测试 QN=20240601085857223;ST=32;CN=3020;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&PolId=w01018;InfoId=i11001;BeginTime=20240601010522,EndTime=20240601085857&&
async Task<List<LogInfo>> Gb_OnGetLogInfos((string? PolId, DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) objects)
{
    return await Task.FromResult(new List<LogInfo>() { new("你好日志1") { PolId = "a1001", DataTime = DateTime.Now }, new("你好日志2") { DataTime = DateTime.Now } });
}

//测试 QN=20240601085857223;ST=32;CN=3019;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&PolId=w01018&&
async Task<string> Gb_OnGetSN((string PolId, RspInfo RspInfo) objects)
{
    return await Task.FromResult("010000A8900016F000169DC0");
}

//测试 QN=20240601085857223;ST=32;CN=3018;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&PolId=w01018&&
async Task<string> Gb_OnGetSampleExtractionTime((string PolId, RspInfo RspInfo) objects)
{
    return await Task.FromResult("30");
}

//测试 QN=20160801085857223;ST=32;CN=3017;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&PolId=w01018&&
async Task<(TimeOnly CstartTime, int Ctime)> Gb_OnGetSamplingPeriod((string PolId, RspInfo RspInfo) objects)
{
    return await Task.FromResult((new TimeOnly(0, 0, 0), 2));
}

//测试 QN=20160801085857223;ST=32;CN=3016;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&PolId=w01018;CstartTime=000000;CTime=2&&
async Task Gb_OnSetSamplingPeriod((string PolId, TimeOnly CstartTime, int Ctime, RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

//测试 QN=20160801085857223;ST=32;CN=3015;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&&&
async Task<(DateTime DataTime, int VaseNo)> Gb_OnOutOfStandardRetentionSample(RspInfo objects)
{
    return await Task.FromResult((DateTime.Now, 2));
}

//测试 QN=20160801085857223;ST=32;CN=3014;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&PolId=w01018&&
async Task Gb_OnComparisonSampling((string PolId, RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

//测试 QN=20240601085857223;ST=32;CN=3013;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&PolId=w01018&&
async Task Gb_OnStartCleaningOrBlowback((string PolId, RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

//测试 QN=20240601085857223;ST=32;CN=3012;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&PolId=w01018&&
async Task Gb_OnRealTimeSampling((string PolId, RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

//测试 QN=20160801085857223;ST=32;CN=3011;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&PolId=w01018&&
//QN=20240601085857223;ST=32;CN=3011;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&PolId=w01018,CalibrationType=0&&
async Task Gb_OnCalibrate((string PolId, CalibrationType? CalibrationType, RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

//测试 QN=20240601085857223;ST=44;CN=2011;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&BeginTime=20240601064500;EndTime=20240601084500&&
Task<List<ElectricityMonitoringHistory>> Gb_OnGetElectricityMonitoringRealTimeData((DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) objects)
{
    return Task.FromResult(new List<ElectricityMonitoringHistory>
    {
        new(DateTime.Now, [new("a1001") { Rtd = "50.4", Flag = "N" }]),
        new(DateTime.Now.AddHours(-1), [new("a1002") { Rtd = "35.2", Flag = "D" }])
    });
}

//测试 QN=20160801085857223;ST=32;CN=2041;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&BeginTime=20160801000000,EndTime=20160801000000&&
async Task<List<RunningTimeHistory>> Gb_OnGetRunningTimeData((DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) objects)
{
    return await Task.FromResult(new List<RunningTimeHistory>() { new(DateTime.Now, [new("SB1", "1")]) });
}

//测试 QN=20240601085857223;ST=32;CN=2013;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&BeginTime=20240601080000;EndTime=20240601080000&& 
async Task<List<HistoryRawData>> Gb_OnGetRawData((DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) objects)
{
    return await Task.FromResult(new List<HistoryRawData>
    {
        new(DateTime.Now, [new("a1001") { SampleTime= "20240601090109", CompleteTime= "20240601094200", SampleType = "1", Avg = "17.5", Info = [new("a", "1"), new("b", "2")],Flag="N" }]),
        new(DateTime.Now.AddHours(-1), [new("a1002") { SampleTime= "20240601080109", CompleteTime= "20240601084200", SampleType = "1", Avg = "17.5", Info = [new("c", "3"), new("d", "4")] }])
    });
}

//测试 QN=20240601085857223;ST=32;CN=2031;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&BeginTime=20240601000000;EndTime=20240601000000&&
async Task<(List<HistoryData> HistoryDatas, bool ReturnValue, int? Timeout)> Gb_OnGetDayData((DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) objects)
{
    return await Task.FromResult((new List<HistoryData> {
        new(DateTime.Now, [new("a1001") { Min = "30.7", Avg = "50.4", Max = "60.5", Flag = "N" }]),
        new(DateTime.Now.AddHours(-1), [new("a1002") { Min = "30.7", Avg = "50.4", Max = "60.5", Flag = "N" }])
    }, false, -1));
}

//测试 QN=20240601085857223;ST=32;CN=2061;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&BeginTime=20240601080000;EndTime=20240601080000&&
async Task<(List<HistoryData> HistoryDatas, bool ReturnValue, int? Timeout)> Gb_OnGetHourData((DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) objects)
{
    return await Task.FromResult((new List<HistoryData> {
        new(DateTime.Now, [new("a1001") { Min = "30.7", Avg = "50.4", Max = "60.5", Flag = "N" }]),
        new(DateTime.Now.AddHours(-1), [new("a1002") { Min = "30.7", Avg = "50.4", Max = "60.5", Flag = "N" }])
    }, false, -1));
}

//测试 QN=20240601085857223;ST=31;CN=2051;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&BeginTime=20240601084000;EndTime=20240601084000&&
async Task<(List<HistoryData> HistoryDatas, bool ReturnValue, int? Timeout)> Gb_OnGetMinuteData((DateTime BeginTime, DateTime EndTime, RspInfo RspInfo) objects)
{
    return await Task.FromResult((new List<HistoryData> {
        new(DateTime.Now, [new("a1001") { Min = "30.7", Avg = "50.4", Max = "60.5", Flag = "N" }]),
        new(DateTime.Now.AddHours(-1), [new("a1002") { Min = "30.7", Avg = "50.4", Max = "60.5", Flag = "N" }])
    }, false, -1));
}

//测试 QN=20240601085857223;ST=32;CN=2022;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&&&
async Task Gb_OnStopRunningStateData(RspInfo objects)
{
    await Task.CompletedTask;
}

//测试 QN=20240601085857223;ST=32;CN=2021;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&&&
async Task Gb_OnStartRunningStateData(RspInfo objects)
{
    await Task.CompletedTask;
}

//测试 QN=20240601085857223;ST=32;CN=2012;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&&&
async Task Gb_OnStopRealTimeData(RspInfo objects)
{
    await Task.CompletedTask;
}

//测试 QN=20240601085857223;ST=32;CN=2011;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&&&
async Task Gb_OnStartRealTimeData(RspInfo objects)
{
    await Task.CompletedTask;
}

//测试 QN=20240601085857223;ST=32;CN=1072;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&NewPW=654321&&
async Task Gb_OnSetNewPW((string NewPW, RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

//测试 QN=20240601085857223;ST=31;CN=1064;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&MinInterval=10&&
async Task Gb_OnSetMinuteDataInterval((int MinInterval, RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

//测试 QN=20240601085857223;ST=31;CN=1063;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&&&
Task<int> Gb_OnGetMinuteDataInterval(RspInfo objects)
{
    return Task.FromResult(10);
}

//测试 QN=20240601085857223;ST=32;CN=1062;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&RtdInterval=30&&
Task Gb_OnSetRealTimeDataInterval((int RtdInterval, RspInfo RspInfo) objects)
{
    return Task.CompletedTask;
}

//测试 QN=20240601085857223;ST=32;CN=1061;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&&&
Task<int> Gb_OnGetRealTimeDataInterval(RspInfo objects)
{
    return Task.FromResult(5);
}

//测试 QN=20240601085857223;ST=32;CN=1014;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&SKCreateTime=20240601085857223,NewSK=0000000000000000&&
async Task Gb_OnSetNewSK((string SKCreateTime, string NewSK, RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

//测试 QN=20240601085857223;ST=32;CN=1012;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&PolId=w01018;SystemTime=20240601085857&&
async Task Gb_OnSetSystemTime((string? PolId, DateTime SystemTime, RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

//测试 QN=20240601085857223;ST=32;CN=1011;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&PolId=w01018&&
async Task<DateTime?> Gb_OnGetSystemTime((string? PolId, RspInfo RspInfo) objects)
{
    //回复空，则取系统时间回复
    return await Task.FromResult<DateTime?>(default);
}

//测试 QN=20240601085857223;ST=32;CN=1002;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&i23005-Info=6970000000001ABCDEFG1234&&
async Task Gb_OnSetMN((string? DataLoggerId, RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

//测试 QN=20240601085857223;ST=32;CN=1000;PW=123456;MN=010000A8900016F000169DC0;Flag=9;CP=&&OverTime=5;ReCount=3&&
async Task Gb_OnSetOverTimeAndReCount((int OverTime, int ReCount, RspInfo RspInfo) objects)
{
    await Task.CompletedTask;
}

await gb.OpenAsync();

//try
//{
//    //测试 QN=20240601085857223;ST=91;CN=9013;PW=123456;MN=;Flag=8;CP=&&&&
//    await gb.UploadHardwareSN("6970000000001ABCDEFG1234", "A800200A8C6D", "E86A643C7BBC", "A763742C1AE1");
//}
//catch (TimeoutException) { }

//try
//{
//    //测试 QN=20240601085857223;ST=91;CN=9013;PW=123456;MN=010000A8900016F000169DC0;Flag=8;CP=&&&& 
//    await gb.AskSetSystemTime("a1001");
//}
//catch (TimeoutException) { }

//try
//{
//    //测试 QN=20240601085857223;ST=91;CN=9013;PW=123456;MN=010000A8900016F000169DC0;Flag=8;CP=&&&&
//    await gb.AskNewSK();
//}
//catch (TimeoutException) { }

//List<RealTimeData> realTimeDatas =
//[
//    new RealTimeData("a1001"){Rtd="50.4",Flag="N"},
//    new RealTimeData("a1002"){Rtd="35.2",Flag="D",Other=new(){{"cd","adf"},{"cdd","13"}}}
//];
//try
//{
//    //测试 QN=20240601085857223;ST=91;CN=9014;PW=123456;MN=010000A8900016F000169DC0;Flag=8;CP=&&&&
//    await gb.UploadRealTimeData(DateTime.Now, realTimeDatas);
//}
//catch (TimeoutException) { }
//await gb.SendRealTimeData(DateTime.Now, realTimeDatas);

//try
//{
//    //测试 QN=20240429114224393;ST=91;CN=9014;PW=123456;MN=010000A8900016F000169DC0;Flag=8;CP=&&&&
//    await gb.UploadRunningStateData(DateTime.Now, [new RunningStateData("SB1", "1"), new RunningStateData("SB2", "2")]);
//}
//catch (TimeoutException) { }

//List<StatisticsData> statisticsDatas =
//[
//    new StatisticsData("a1001"){Min="30.7",Avg="50.4",Max="60.5",Flag="N"},
//    new StatisticsData("a1002"){Min="10.2",Avg="35.2",Max="50.1",Flag="D",Other=new(){{"cd","adf"},{"cdd","13"}}}
//];
//try
//{
//    //测试 QN=20240430102725064;ST=91;CN=9014;PW=123456;MN=010000A8900016F000169DC0;Flag=8;CP=&&&&
//    await gb.UploadMinuteData(DateTime.Now, statisticsDatas);
//}
//catch (Exception ex)
//{
//    await gb.ReissueStatisticsData(ex.Data["SendCmd"]?.ToString() ?? "");
//}
//await gb.SendMinuteData(DateTime.Now, statisticsDatas);

//try
//{
//    //测试 QN=20240430102725064;ST=91;CN=9014;PW=123456;MN=010000A8900016F000169DC0;Flag=8;CP=&&&&
//    await gb.UploadHourData(DateTime.Now, statisticsDatas);
//}
//catch (Exception ex)
//{
//    await gb.ReissueStatisticsData(ex.Data["SendCmd"]?.ToString() ?? "");
//}
//await gb.SendHourData(DateTime.Now, statisticsDatas);

//try
//{
//    //测试 QN=20240430102725064;ST=91;CN=9014;PW=123456;MN=010000A8900016F000169DC0;Flag=8;CP=&&&&
//    await gb.UploadDayData(DateTime.Now, statisticsDatas);
//}
//catch (Exception ex)
//{
//    await gb.ReissueStatisticsData(ex.Data["SendCmd"]?.ToString() ?? "");
//}
//await gb.SendDayData(DateTime.Now, statisticsDatas);

//try
//{
//    //测试 QN=20240429114224393;ST=91;CN=9014;PW=123456;MN=010000A8900016F000169DC0;Flag=8;CP=&&&&
//    await gb.UploadRunningTimeData(DateTime.Now, [new RunningTimeData("SB1", "1"), new RunningTimeData("SB2", "2")]);
//}
//catch (TimeoutException) { }

//try
//{
//    //测试 QN=20160801085857223;ST=91;CN=9014;PW=123456;MN=010000A8900016F000169DC0;Flag=8;CP=&&&&
//    await gb.UploadAcquisitionDeviceRestartTime(DateTime.Now, DateTime.Now);
//}
//catch (TimeoutException) { }

//try
//{
//    //测试 QN=20160801085857223;ST=91;CN=9014;PW=123456;MN=010000A8900016F000169DC0;Flag=8;CP=&&&&
//    await gb.UploadFurnaceTemperature(DateTime.Now, "868");
//}
//catch (TimeoutException) { }

//try
//{
//    //测试 QN=20160801085857223;ST=91;CN=9014;PW=123456;MN=010000A8900016F000169DC0;Flag=4;CP=&&&&
//    await gb.UploadSN(DateTime.Now, "a1001", "010000A8900016F000169DC0");
//}
//catch (TimeoutException) { }

//try
//{
//    //测试 QN=20160801085857223;ST=91;CN=9014;PW=123456;MN=010000A8900016F000169DC0;Flag=4;CP=&&&&
//    await gb.UploadLog(DateTime.Now, "a1001", "你好日志");
//}
//catch (TimeoutException) { }

//try
//{
//    //测试 QN=20160801085857223;ST=91;CN=9014;PW=123456;MN=010000A8900016F000169DC0;Flag=4;CP=&&&&
//    await gb.UploadInfo(DateTime.Now, "a1001", new List<DeviceInfo> { new("i12001", "1"), new("i12002", "1") }, 120000);
//}
//catch (TimeoutException) { }

Console.WriteLine("OK");
Console.ReadLine();