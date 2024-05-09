## HJ212国标协议：

``` C#
IGB gb = new GB("输出一", new SerialPort(), "88888888");
//比如上传实时数据
await gb.UploadRealTimeData(DateTime.Now, [new("a1001") { Rtd = "20.5", Flag = "N" }]);

//若系统想支持国标的平台来获取时间，只需要加上如下代码即可
gb.OnGetSystemTime += Gb_OnGetSystemTime;

async Task<DateTime?> Gb_OnGetSystemTime((string? PolId, HJ212.Model.RspInfo RspInfo) objects)
{
    return await Task.FromResult(DateTime.Now);
}
```
更多详见Test例子，例子中附带测试命令
