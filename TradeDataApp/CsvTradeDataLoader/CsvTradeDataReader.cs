using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TradeDataCommon;

namespace CsvTradeDataPlugin
{
    public class CsvTradeDataReader : ITradeDataReader
    {
        public string FileExtension
        {
            get
            {
                return ".csv";
            }
        }

        public char Separator
        {
            get
            {
                return ',';
            }
        }

        public IEnumerable<ITradeData> ReadTradeData(TextReader textReader)
        {
            string line;
            int rowNo = 1;
            while ((line = textReader.ReadLine()) != null)
            {
                ITradeData tradeData = null;
                try
                {
                    string[] values = line.Split(new char[] { Separator });
                    if (values.Length > 6) throw new FormatException("Too many columns in a row.");
                    tradeData = new TradeData(values[0], values[1], values[2], values[3], values[4], values[5]).CheckDataBusinessCorrectness();
                }
                catch (Exception e)
                {
                    e.Data["rowNo"] = rowNo;
                    Trace.TraceError("Exception when processing row number: " + rowNo + " (" + e.Message + ")");
                    throw e;
                }
                rowNo++;
                yield return tradeData;
            }
        }

    }
}
