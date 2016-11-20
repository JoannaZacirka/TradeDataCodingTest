using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using TradeDataApp.Model;
using TradeDataCommon;

namespace TradeDataApp.ViewModel
{
    public class MainViewModel 
    {
        public ObservableCollection<TradeDataReadingResult> FileReadingResults { get; set; }

        private IDirectoryMonitor PluginDirectoryMonitor;
        private IDirectoryMonitor DataDirectoryMonitor;
        private IReaderTypesManager ReaderTypesManager;
        private Timer LoadDataTimer;
        public Action<Action> RunOnUiThread;

        public MainViewModel()
        {
            FileReadingResults = new ObservableCollection<TradeDataReadingResult>();
            PluginDirectoryMonitor = new DirectoryMonitor(AppSettingsProvider.PluginsDirectory, "*.dll");
            DataDirectoryMonitor = new DirectoryMonitor(AppSettingsProvider.InputDirectory);
            ReaderTypesManager = new ReaderTypesManager();
            LoadDataTimer = new Timer(LoadDataCallback, null, AppSettingsProvider.CheckingIntervalMs, Timeout.Infinite);
        }

        void LoadDataCallback(Object state)
        {
            try
            {
                var newPluginFiles = PluginDirectoryMonitor.GetUnprocessedFileNames();
                foreach (var pluginFile in newPluginFiles)
                {
                    var readerTypes = AssemblyTypesSearcher.GetTypesImplementing<ITradeDataReader>(pluginFile);
                    var registered = ReaderTypesManager.RegisterReaderTypes(readerTypes);
                    PluginDirectoryMonitor.MarkFileName(pluginFile, registered > 0);
                }

                var newDataFiles = DataDirectoryMonitor.GetUnprocessedFileNames().AsParallel();
                newDataFiles.ForAll(fileName => LoadFileData(fileName));
            }
            catch (Exception e)
            {
                RunOnUiThread(() => NotifyException(e));
            }
            finally
            {
                LoadDataTimer.Change(AppSettingsProvider.CheckingIntervalMs, Timeout.Infinite);
            }
        }

        private void NotifyException(Exception e)
        {
            MessageBox.Show(e.Message, "Exception occured", MessageBoxButton.OK, MessageBoxImage.Error);
            Trace.TraceError(e.Message);
            Trace.Flush();
        }

        private void LoadFileData(string path)
        {
            try
            {
                var extension = System.IO.Path.GetExtension(path);
                var reader = ReaderTypesManager.GetReader(extension);
                if (reader != null)
                {
                    List<ITradeData> tradeDataList = new List<ITradeData>();
                    using (TextReader textReader = new StreamReader(path))
                    {
                        var tradeData = reader.ReadTradeData(textReader);
                        if (tradeData != null)
                        {
                            tradeDataList.AddRange(tradeData);
                        }
                    }
                    RunOnUiThread(() => ShowLoadFileDataResult(path, tradeDataList));
                    DataDirectoryMonitor.MarkFileNameAsProcessed(path);

                }
            }
            catch (Exception e)
            {
                var msg = e.Data.Contains("rowNo") ? "RowNumber=" + e.Data["rowNo"] + ": " + e.Message : e.Message;
                RunOnUiThread(() => ShowLoadFileDataResult(path, null, msg));
                Trace.TraceError("Exception occured when processing the file: " + path + Environment.NewLine + msg);
                Trace.Flush();
                DataDirectoryMonitor.MarkFileNameAsInvalid(path);
            }
        }

        private void ShowLoadFileDataResult(string path, IEnumerable<ITradeData> tradeData, string statusMessage = "Sucess")
        {
            var fileName = Path.GetFileName(path);
            var readingResult = new TradeDataReadingResult(fileName, tradeData, statusMessage);
            FileReadingResults.Add(readingResult);
        }

    }
}
