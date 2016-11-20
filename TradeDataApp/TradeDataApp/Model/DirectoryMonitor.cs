using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeDataApp.Model
{
    public class DirectoryMonitor : IDirectoryMonitor
    {
        private ConcurrentBag<string> ProcessedFileNames = new ConcurrentBag<string>();
        private ConcurrentBag<string> InvalidFileNames = new ConcurrentBag<string>();

        public string DirectoryName { get; private set; }
        public string SearchPattern { get; set; }

        public DirectoryMonitor(string directoryName, string searchPattern = null)
        {
            DirectoryName = directoryName;
            SearchPattern = searchPattern;
        }

        public IEnumerable<string> GetUnprocessedFileNames(bool withInvalid = false)
        {
            var allFileNames = SearchPattern != null ? Directory.GetFiles(DirectoryName, SearchPattern) :
                Directory.GetFiles(DirectoryName);

            return (from fileName in allFileNames
                    where !ProcessedFileNames.Contains(fileName) &&
                    (withInvalid || !InvalidFileNames.Contains(fileName)) 
                    select fileName).ToList();
        }

        public void MarkFileNameAsProcessed(string fileName)
        {
            ProcessedFileNames.Add(fileName);
        }

        public void MarkFileNameAsInvalid(string fileName)
        {
            InvalidFileNames.Add(fileName);
        }

        public void MarkFileName(string fileName, bool processed)
        {
            if (processed)
                MarkFileNameAsProcessed(fileName);
            else
                MarkFileNameAsInvalid(fileName);
        }
    }
}
