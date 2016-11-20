using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using TradeDataApp.Model;

namespace TradeDataAppTests
{
    [TestClass]
    public class DirectoryMonitorTests
    {
        private static string DirectoryName = "MonitorTestDirectory";
        private static string TestFileName1 = DirectoryName + "\\TestFile1.txt";
        private static string TestFileName2 = DirectoryName + "\\TestFile2.csv";
        private static string TestFileName3 = DirectoryName + "\\TestFile3.xml";

        [TestInitialize]
        public void Setup()
        {
            Directory.CreateDirectory(DirectoryName);
            var fs1 = File.Create(TestFileName1);
            fs1.Close();
            var fs2 = File.Create(TestFileName2);
            fs2.Close();
        }

        [TestMethod]
        public void TestSeeFilesInDirectory()
        {
            var monitor = new DirectoryMonitor(DirectoryName);
            var fileNames = monitor.GetUnprocessedFileNames().OrderBy(s => s).ToList();

            Assert.AreEqual<int>(fileNames.Count, 2);
            Assert.AreEqual<string>(fileNames[0], TestFileName1);
            Assert.AreEqual<string>(fileNames[1], TestFileName2);
        }

        [TestMethod]
        public void TestFiltersFilesFromDirectory()
        {
            var monitor = new DirectoryMonitor(DirectoryName, "*.csv");
            var fileNames = monitor.GetUnprocessedFileNames().OrderBy(s => s).ToList();

            Assert.AreEqual<int>(fileNames.Count, 1);
            Assert.AreEqual<string>(fileNames[0], TestFileName2);
        }

        [TestMethod]
        public void TestDoesNotSeeProcessedFiles()
        {
            var monitor = new DirectoryMonitor(DirectoryName);
            monitor.MarkFileNameAsProcessed(TestFileName2);
            var fileNames = monitor.GetUnprocessedFileNames().OrderBy(s => s).ToList();

            Assert.AreEqual<int>(fileNames.Count, 1);
            Assert.AreEqual<string>(fileNames[0], TestFileName1);
        }

        [TestMethod]
        public void TestDoesNotSeeInvalidFiles()
        {
            var monitor = new DirectoryMonitor(DirectoryName);
            monitor.MarkFileNameAsInvalid(TestFileName1);
            var fileNames = monitor.GetUnprocessedFileNames().OrderBy(s => s).ToList();

            Assert.AreEqual<int>(fileNames.Count, 1);
            Assert.AreEqual<string>(fileNames[0], TestFileName2);
        }

        [TestMethod]
        public void TestSeesAlsoInvalidFiles()
        {
            var monitor = new DirectoryMonitor(DirectoryName);
            monitor.MarkFileNameAsInvalid(TestFileName1);
            var fileNames = monitor.GetUnprocessedFileNames(true).OrderBy(s => s).ToList();

            Assert.AreEqual<int>(fileNames.Count, 2);
            Assert.AreEqual<string>(fileNames[0], TestFileName1);
            Assert.AreEqual<string>(fileNames[1], TestFileName2);
        }

        [TestMethod]
        public void SeesOnlyNewFiles()
        {
            var monitor = new DirectoryMonitor(DirectoryName);
            monitor.MarkFileNameAsProcessed(TestFileName1);
            monitor.MarkFileNameAsInvalid(TestFileName2);
            var fileNames = monitor.GetUnprocessedFileNames().OrderBy(s => s).ToList();

            Assert.AreEqual<int>(fileNames.Count, 0);

            var fs3 = File.Create(TestFileName3);
            fs3.Close();

            fileNames = monitor.GetUnprocessedFileNames().OrderBy(s => s).ToList();
            Assert.AreEqual<int>(fileNames.Count, 1);
            Assert.AreEqual<string>(fileNames[0], TestFileName3);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(DirectoryName, true);
        }

    }
}
