using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class SendRealTimeDataReq : IByteStream
    {
        private readonly string _mn;
        private readonly bool _flag;
        private readonly string _pw;
        private readonly bool _qn;
        private readonly ST _st;
        private readonly DateTime _dataTime;
        private readonly Dictionary<string, (string? value, string? flag)> _data;

        public SendRealTimeDataReq(string mn, bool flag, string pw, bool qn, ST st, DateTime dataTime, Dictionary<string, (string? value, string? flag)> data)
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
            var rs = $"{(_qn ? $"QN={DateTime.Now:yyyyMMddHHmmssfff};" : "")}ST={(int)_st};CN={(int)CN.实时数据};PW={_pw};MN={_mn};CP=&&DataTime={_dataTime:yyyyMMddHHmmss},{string.Join(";", _data.Select(c => $"{c.Key}-Rtd={c.Value.value}{(_flag ? $",{c.Key}-Flag={c.Value.flag}" : "")}"))}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
