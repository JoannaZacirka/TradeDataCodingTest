using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeDataCommon;

namespace TradeDataApp.ViewModel
{
    public class TradeDataReadingResult
    {
        public string FileName { get; private set; }
        public IEnumerable<ITradeData> TradeDataList { get; private set; }
        public string Status { get; private set; }

        public TradeDataReadingResult(string fileName, IEnumerable<ITradeData> tradeData, string status)
        {
            FileName = fileName;
            TradeDataList = tradeData;
            Status = status;
        }

    }
}
