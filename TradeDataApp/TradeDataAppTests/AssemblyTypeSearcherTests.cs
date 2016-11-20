using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeDataApp.Model;
using TradeDataCommon;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TradeDataAppTests
{
    public class DummyTradeDataReader : ITradeDataReader
    {
        public string FileExtension
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<ITradeData> ReadTradeData(TextReader textReader)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class DummyAbstractTradeDataReader : ITradeDataReader
    {
        public string FileExtension
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<ITradeData> ReadTradeData(TextReader textReader)
        {
            throw new NotImplementedException();
        }
    }

    public interface DummyTradeDataReaderInterfac : ITradeDataReader
    {
    }


    [TestClass]
    public class AssemblyTypeSearcherTests
    {
        [TestMethod]
        public void FindTradeDataReadersInCurrentAssembly()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var readerTypes = AssemblyTypesSearcher.GetTypesImplementing<ITradeDataReader>(assembly.Location).ToList();

            Assert.AreEqual<int>(1, readerTypes.Count());
            Assert.AreEqual<Type>(typeof(DummyTradeDataReader), readerTypes[0]);
        }
    }
}
