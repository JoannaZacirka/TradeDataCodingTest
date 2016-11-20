using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeDataCommon
{
    public interface ITradeDataReader
    {
        string FileExtension { get; }

        IEnumerable<ITradeData> ReadTradeData(TextReader textReader);
    }
}
