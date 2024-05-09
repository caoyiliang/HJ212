namespace HJ212;

/// <summary>请求数据事件处理</summary>
public delegate Task<T1> ActivelyAskDataEventHandler<T, T1>(T objects);
