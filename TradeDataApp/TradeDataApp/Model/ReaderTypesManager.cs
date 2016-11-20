using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TradeDataCommon;

namespace TradeDataApp.Model
{
    public class ReaderTypesManager : IReaderTypesManager
    {
        private Dictionary<string, ITradeDataReader> LoadersDictionary = new Dictionary<string, ITradeDataReader>();

        public ITradeDataReader GetReader(string extension)
        {
            if (LoadersDictionary.Keys.Contains(extension))
            {
                return LoadersDictionary[extension];
            }
            else
            {
                Trace.TraceWarning(string.Format("Unknown file extension ({0}). Please register supporting plugin.", extension));
                Trace.Flush();
                return null;
            }
        }

        public int RegisterReaderTypes(IEnumerable<Type> readerTypes)
        {
            int readersRegistered = 0;
            if (readerTypes != null)
            {
                foreach (Type type in readerTypes)
                {
                    try
                    {
                        ITradeDataReader reader = (ITradeDataReader)Activator.CreateInstance(type);
                        LoadersDictionary[reader.FileExtension] = reader;
                        readersRegistered++;
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError("Exception occured when registering reader: " + type.FullName + Environment.NewLine + e.Message);
                        Trace.Flush();
                    }
                }
            }
            return readersRegistered;
        }

    }
}
