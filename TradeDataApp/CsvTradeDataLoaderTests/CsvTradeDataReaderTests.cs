using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeDataCommon;
using System.IO;
using CsvTradeDataPlugin;

namespace CsvTradeDataPluginTests
{
    [TestClass]
    public class CsvTradeDataReaderTests
    {
        ITradeDataReader Reader { get; set; }

        [TestInitialize]
        public void Setup()
        {
            Reader = new CsvTradeDataReader();
        }

        [TestMethod]
        public void TestExampleXml()
        {
            var exampleXml = @"2013-5-20,30.16,30.39,30.02,30.17,1478200
2013-5-17,29.77,30.26,29.77,30.26,2481400
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

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestMissingData()
        {
            var exampleXml = @"2013-5-20,30.16,30.39,30.02,30.17,1478200
2013-5-17,29.77,30.26,29.77,30.26
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

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestTooManyColumns()
        {
            var exampleXml = @"2013-5-20,30.16,30.39,30.02,30.17,1478200
2013-5-17,29.77,30.26,29.77,30.26,2481400, 9
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


        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestWrongDateFormat()
        {
            var exampleXml = @"2013-5-20,30.16,30.39,30.02,30.17,1478200
2013-15-17,29.77,30.26,29.77,30.26,2481400
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


        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestWrongDecimalFormat()
        {
            var exampleXml = @"2013-5-20,30.16,30.39,30.02,30.17,1478200
2013-5-17,29.7.7,30.26,29.77,30.26,2481400
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

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestNegativeDecimalNumber()
        {
            var exampleXml = @"2013-5-20,30.16,30.39,30.02,30.17,1478200
2013-5-17,29.77,-30.26,29.77,30.26,2481400
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
            Assert.AreEqual<decimal>(td.Close, -30.26m);
            Assert.AreEqual<long>(td.Volume, 2481400);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestDecimalWithTooManyDecimalPlaces()
        {
            var exampleXml = @"2013-5-20,30.16,30.39,30.02,30.17,1478200
2013-5-17,29.77,30.266,29.77,30.26,2481400
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

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestNegativeVolume()
        {
            var exampleXml = @"2013-5-20,30.16,30.39,30.02,30.17,1478200
2013-5-17,29.77,30.26,29.77,30.26,-2481400
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

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestWrongLongFormat()
        {
            var exampleXml = @"2013-5-20,30.16,30.39,30.02,30.17,1478200
2013-5-17,29.77,30.26,29.77,30.26,2481_400
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

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void TestLongOverflow()
        {
            var exampleXml = @"2013-5-20,30.16,30.39,30.02,30.17,1478200
2013-5-17,29.77,30.26,29.77,30.26,92233720368547758070
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

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void TestDecimalOverflow()
        {
            var exampleXml = @"2013-5-20,30.16,30.39,30.02,30.17,1478200
2013-5-17,79228162514264337593543950336.77,30.26,29.77,30.26,2481400
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





    }
}
