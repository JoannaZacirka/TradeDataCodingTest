using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TradeDataCommon;

/// <summary>
/// Trade data loader from semicolon-separated values file
/// </summary>
namespace TextTradeDataPlugin
{
    public class TextTradeDataReader : ITradeDataReader
    {
        public string FileExtension
        {
            get
            {
                return ".txt";
            }
        }

        public char Separator
        {
            get
            {
                return ';';
            }
        }

        private class ColumnIndexes
        {
            public int DateIndex { get; set; }
            public int OpenIndex { get; set; }
            public int HighIndex { get; set; }
            public int LowIndex { get; set; }
            public int CloseIndex { get; set; }
            public int VolumeIndex { get; set; }

            public static ColumnIndexes GeFromHeaderLine(string headerLine, char separator)
            {
                ColumnIndexes indexes = new ColumnIndexes();

                var columns = headerLine.Split(new char[] { separator });
                indexes.DateIndex = Array.IndexOf<string>(columns, "Date");
                indexes.OpenIndex = Array.IndexOf<string>(columns, "Open");
                indexes.HighIndex = Array.IndexOf<string>(columns, "High");
                indexes.LowIndex = Array.IndexOf<string>(columns, "Low");
                indexes.CloseIndex = Array.IndexOf<string>(columns, "Close");
                indexes.VolumeIndex = Array.IndexOf<string>(columns, "Volume");
                return indexes;
            }
        }

        public IEnumerable<ITradeData> ReadTradeData(TextReader textReader)
        {
            string line = textReader.ReadLine();
            var cIdx = ColumnIndexes.GeFromHeaderLine(line, Separator);
            int rowNo = 2;
            while ((line = textReader.ReadLine()) != null)
            {
                ITradeData tradeData = null;
                try
                {
                    string[] values = line.Split(new char[] { Separator });
                    if (values.Length > 6) throw new FormatException("Too many columns in a row.");
                    tradeData = new TradeData(values[cIdx.DateIndex], values[cIdx.OpenIndex], values[cIdx.HighIndex], values[cIdx.LowIndex], values[cIdx.CloseIndex], values[cIdx.VolumeIndex]).CheckDataBusinessCorrectness();
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
