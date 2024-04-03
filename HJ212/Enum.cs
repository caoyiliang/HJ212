using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HJ212
{
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

    public enum CN
    {
        实时数据 = 2011,
        分钟数据 = 2051,
        小时数据 = 2061,
        日历史数据 = 2031
    }
}
