using System;

namespace TradeDataCommon
{
    public interface ITradeData
    {
        DateTime Date { get; }
        decimal Open { get; }
        decimal High { get; }
        decimal Low { get; }
        decimal Close { get; }
        long Volume { get; }
    }

}