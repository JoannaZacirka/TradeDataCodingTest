using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeDataCommon
{
    public class TradeData : ITradeData
    {
        public DateTime Date { get; private set; }

        public decimal Open { get; private set; }

        public decimal Close { get; private set; }

        public decimal High { get; private set; }

        public decimal Low { get; private set; }

        public long Volume { get; private set; }

        public TradeData(DateTime date, decimal open, decimal high, decimal low, decimal close, long volume)
        {
            Date = date;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        public TradeData(string date, string open, string high, string low, string close, string volume) 
        {
            Date = DateTime.Parse(date);
            Open = Decimal.Parse(open);
            High = Decimal.Parse(high);
            Low = Decimal.Parse(low);
            Close = Decimal.Parse(close);
            Volume = long.Parse(volume);
        }

        private void CheckDecimal(decimal d)
        {
            if (d < 0) throw new FormatException("Negative decimal");
            if (d - decimal.Round(d, 2) != 0) throw new FormatException("Decimal with too many decimal places");
        }

        private void CheckLong(long d)
        {
            if (d < 0) throw new FormatException("Negative long");
        }

        public TradeData CheckDataBusinessCorrectness()
        {
            CheckDecimal(Open);
            CheckDecimal(High);
            CheckDecimal(Low);
            CheckDecimal(Close);
            CheckLong(Volume);
            return this;
        }


    }
}
