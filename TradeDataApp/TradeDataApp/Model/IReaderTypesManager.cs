using System;
using System.Collections.Generic;
using TradeDataCommon;

namespace TradeDataApp.Model
{
    interface IReaderTypesManager
    {
        int RegisterReaderTypes(IEnumerable<Type> readerTypes);
        ITradeDataReader GetReader(string extension);
    }
}
