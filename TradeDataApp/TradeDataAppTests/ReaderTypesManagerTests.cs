using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using TradeDataApp.Model;
using TradeDataCommon;
using System.Collections.Generic;

namespace TradeDataAppTests
{
    [TestClass]
    public class ReaderTypesManagerTests
    {
        private static string XmlExtension = ".xml";
        private static string CsvExtension = ".csv";

        private class DummyXmlTradeDataReader : ITradeDataReader
        {
            public string FileExtension { get { return ReaderTypesManagerTests.XmlExtension; } }
            public IEnumerable<ITradeData> ReadTradeData(TextReader textReader)
            {
                throw new NotImplementedException();
            }
        }

        private class DummyCsvTradeDataReader : ITradeDataReader
        {
            public string FileExtension { get { return ReaderTypesManagerTests.CsvExtension; } }

            public IEnumerable<ITradeData> ReadTradeData(TextReader textReader)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void TestRegisterReaderTypes()
        {
            ReaderTypesManager manager = new ReaderTypesManager();
            var registeredCnt = manager.RegisterReaderTypes(new[] { typeof(DummyXmlTradeDataReader), typeof(DummyCsvTradeDataReader) });

            Assert.AreEqual<int>(2, registeredCnt);
        }

        [TestMethod]
        public void GetXmlReaderType()
        {
            ReaderTypesManager manager = new ReaderTypesManager();
            var registeredCnt = manager.RegisterReaderTypes(new[] { typeof(DummyXmlTradeDataReader), typeof(DummyCsvTradeDataReader) });

            var reader = manager.GetReader(XmlExtension);
            Assert.AreEqual<string>(XmlExtension, reader.FileExtension);
            Assert.AreEqual<Type>(typeof(DummyXmlTradeDataReader), reader.GetType());
        }

        [TestMethod]
        public void GetCsvReaderType()
        {
            ReaderTypesManager manager = new ReaderTypesManager();
            var registeredCnt = manager.RegisterReaderTypes(new[] { typeof(DummyXmlTradeDataReader), typeof(DummyCsvTradeDataReader) });

            var reader = manager.GetReader(CsvExtension);
            Assert.AreEqual<string>(CsvExtension, reader.FileExtension);
            Assert.AreEqual<Type>(typeof(DummyCsvTradeDataReader), reader.GetType());
        }

    }



}    
