using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeDataCommon;
using System.IO;

namespace TextTradeDataPluginTests
{
    [TestClass]
    public class TextTradeDataReaderTests
    {
        ITradeDataReader Reader { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Reader = new TextTradeDataPlugin.TextTradeDataReader();
        }

        [TestMethod]
        public void TestExampleXml()
        {
            var exampleXml = @"Date;Open;High;Low;Close;Volume
2013-5-20;30.16;30.39;30.02;30.17;1478200
2013-5-17;29.77;30.26;29.77;30.26;2481400
2013-5-16;29.78;29.94;29.55;29.67;1077000
2013-5-15;29.63;29.99;29.63;29.98;928700
2013-5-14;29.53;29.77;29.48;29.6;1065900
2013-5-13;29.33;29.59;29.09;29.51;1005800
2013-5-10;29.89;29.91;29.52;29.83;1831000
2013-5-9;29.65;29.68;29.2;29.3;1693600
2013-5-8;29.65;29.99;29.54;29.72;1009800
2013-5-7;29.46;29.62;29.18;29.49;1390200
2013-5-6;28.8;28.9;28.64;28.78;744800
2013-5-3;28.85;29.02;28.45;28.56;1087800
2013-5-2;28.1;28.33;28;28.24;1472500
2013-5-1;27.94;28.19;27.61;27.7;1006900
";

            var reader = new StringReader(exampleXml);
            var tradeData = Reader.ReadTradeData(reader);

            var it = tradeData.GetEnumerator();
            it.MoveNext();
            var td = it.Current;
            Assert.AreEqual<DateTime>(td.Date, new DateTime(2013, 5, 20));
            Assert.AreEqual<decimal>(td.Open, 30.16m);
            Assert.AreEqual<decimal>(td.High, 30.39m);
            Assert.AreEqual<decimal>(td.Low, 30.02m);
            Assert.AreEqual<decimal>(td.Close, 30.17m);
            Assert.AreEqual<long>(td.Volume, 1478200);

            it.MoveNext();
            td = it.Current;
            Assert.AreEqual<DateTime>(td.Date, new DateTime(2013, 5, 17));
            Assert.AreEqual<decimal>(td.Open, 29.77m);
            Assert.AreEqual<decimal>(td.High, 30.26m);
            Assert.AreEqual<decimal>(td.Low, 29.77m);
            Assert.AreEqual<decimal>(td.Close, 30.26m);
            Assert.AreEqual<long>(td.Volume, 2481400);

        }

        //TODO: Write more tests analogical to those in CsvTradeDataPluginTests.CsvTradeDataReaderTests

    }
}
