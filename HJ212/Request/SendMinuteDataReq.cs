using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class SendMinuteDataReq : IByteStream
    {
        private readonly string _mn;
        private readonly bool _flag;
        private readonly string _pw;
        private readonly bool _qn;
        private readonly ST _st;
        private readonly DateTime _dataTime;
        private readonly Dictionary<string, (string? avgValue, string? max, string? min, string? flag)> _data;
        public SendMinuteDataReq(string mn, bool flag, string pw, bool qn, ST st, DateTime dataTime, Dictionary<string, (string? avgValue, string? max, string? min, string? flag)> data)
        {
            this._mn = mn;
            this._flag = flag;
            this._pw = pw;
            this._qn = qn;
            this._st = st;
            this._dataTime = dataTime;
            this._data = data;
        }

        public byte[] ToBytes()
        {
            var rs = $"{(_qn ? $"QN={DateTime.Now:yyyyMMddHHmmssfff};" : "")}ST={(int)_st};CN={(int)CN.分钟数据};PW={_pw};MN={_mn};CP=&&DataTime={_dataTime:yyyyMMddHHmm00},{string.Join(";", _data.Select(c => $"{c.Key}-Min={c.Value.min},{c.Key}-Avg={c.Value.avgValue},{c.Key}-Max={c.Value.max}{(_flag ? $",{c.Key}-Flag={c.Value.flag}" : "")}"))}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
