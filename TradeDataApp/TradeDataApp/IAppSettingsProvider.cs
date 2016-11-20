using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeDataApp
{
    interface IAppSettingsProvider
    {
        int CheckingIntervalMs { get; }

        string InputDirectory { get; }

        string PlginsDirectory { get; }

    }
}
