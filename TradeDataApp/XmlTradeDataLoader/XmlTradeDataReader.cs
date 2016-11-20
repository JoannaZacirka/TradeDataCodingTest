using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TradeDataCommon;

namespace XmlTradeDataPlugin
{
    public class XmlTradeDataLoader : ITradeDataReader
    {
        public string FileExtension
        {
            get
            {
                return ".xml";
            }
        }

        public IEnumerable<ITradeData> ReadTradeData(TextReader textReader)
        {
            XmlReader reader = XmlReader.Create(textReader);
            reader.MoveToContent();
            reader.ReadToDescendant("value");
            int rowNo = 1;
            do
            {
                ITradeData tradeData = null;
                try
                {
                    var date = reader.GetAttribute("date");
                    var open = reader.GetAttribute("open");
                    var high = reader.GetAttribute("high");
                    var low = reader.GetAttribute("low");
                    var close = reader.GetAttribute("close");
                    var volume = reader.GetAttribute("volume");
                    tradeData = new TradeData(date, open, high, low, close, volume).CheckDataBusinessCorrectness();
                }
                catch (Exception e)
                {
                    e.Data["rowNo"] = rowNo;
                    Trace.TraceError("Exception when processing row number: " + rowNo + " (" + e.Message + ")");
                    throw e;
                }
                rowNo++;
                yield return tradeData;
            } while (reader.ReadToNextSibling("value"));
        }
    }
}
