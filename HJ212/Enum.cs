﻿namespace HJ212
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    /// <summary>
    /// 系统编码
    /// </summary>
    public enum ST
    {
        地表水质量监测 = 21,
        空气质量监测 = 22,
        声环境质量监测 = 23,
        地下水质量监测 = 24,
        土壤质量监测 = 25,
        海水质量监测 = 26,
        挥发性有机物监测 = 27,
        大气环境污染源 = 31,
        地表水体环境污染源 = 32,
        地下水体环境污染源 = 33,
        海洋环境污染源 = 34,
        土壤环境污染源 = 35,
        声环境污染源 = 36,
        振动环境污染源 = 37,
        放射性环境污染源 = 38,
        工地扬尘污染源 = 39,
        电磁环境污染源 = 41,
        烟气排放过程监控 = 51,
        污水排放过程监控 = 52,
        系统交互 = 91
    }

    /// <summary>
    /// 命令编码
    /// </summary>
    public enum CN_Server
    {
        设置超时时间及重发次数 = 1000,
        设置数采仪MN编码 = 1002,
        提取现场机时间 = 1011,
        设置现场机时间 = 1012,
        上位机下发新密钥 = 1014,
        提取实时数据间隔 = 1061,
        设置实时数据间隔 = 1062,
        提取分钟数据间隔 = 1063,
        设置分钟数据间隔 = 1064,
        设置现场机访问密码 = 1072,
        取污染物实时数据 = 2011,
        停止察看污染物实时数据 = 2012,
        取原始监测数据 = 2013,
        取设备运行状态数据 = 2021,
        停止察看设备运行状态 = 2022,
        取污染物日历史数据 = 2031,
        取设备运行时间日历史数据 = 2041,
        取污染物分钟数据 = 2051,
        取噪声单次测量数据 = 2052,
        取污染物小时数据 = 2061,
        取自动标样核查校准数据 = 2062,
        零点校准量程校准 = 3011,
        即时采样 = 3012,
        启动清洗反吹 = 3013,
        比对采样 = 3014,
        超标留样 = 3015,
        设置采样时间周期 = 3016,
        提取采样时间周期 = 3017,
        提取出样时间 = 3018,
        提取设备唯一标识 = 3019,
        提取现场机信息 = 3020,
        设置现场机参数 = 3021,
        下发即时留样任务 = 3022,
        启动标样核查 = 3024,
        通知应答 = 9013,
        数据应答 = 9014
    }

    public enum CN_Client
    {
        上传数采仪硬件序号 = 1001,
        上传现场机时间 = 1011,
        现场机时间校准请求 = 1013,
        现场机获取新密钥 = 1014,
        上传实时数据间隔 = 1061,
        上传分钟数据间隔 = 1063,
        上传污染物实时数据 = 2011,
        上传原始监测数据 = 2013,
        上传设备运行状态数据 = 2021,
        上传污染物日历史数据 = 2031,
        上传设备运行时间日历史数据 = 2041,
        上传污染物分钟数据 = 2051,
        上传噪声单次测量数据 = 2052,
        上传污染物小时数据 = 2061,
        上传自动标样核查校准数据 = 2062,
        上传数采仪开机时间 = 2081,
        上传炉膛温度5min均值 = 2111,
        上传超标留样信息 = 3015,
        上传采样时间周期 = 3017,
        上传出样时间 = 3018,
        上传设备唯一标识 = 3019,
        上传现场机信息 = 3020,
        请求应答 = 9011,
        执行结果 = 9012,
        通知应答 = 9013,
        数据应答 = 9014,
        心跳包 = 9015
    }

    public enum CalibrationType
    {
        零点校准与调整,
        量程校准与调整
    }

    public enum Version
    {
        HJT212_2005 = 0,
        HJT212_2017 = 4,
        HJT212_2025 = 8
    }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
}
