using System.Collections.Generic;

namespace TradeDataApp.Model
{
    interface IDirectoryMonitor
    {
        string DirectoryName { get; }

        string SearchPattern { get; set; }

        IEnumerable<string> GetUnprocessedFileNames(bool withInvalid = false);

        void MarkFileNameAsProcessed(string fileName);

        void MarkFileNameAsInvalid(string fileName);

        void MarkFileName(string fileName, bool processed);
    }
}
