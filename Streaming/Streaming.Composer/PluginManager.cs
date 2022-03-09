using Streaming.BusinessLogic;

using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;

namespace Streaming.Composer
{
    public class PluginManager
    {
        private static object lockObj = new object();

        private static string _path;
        private static CompositionContainer _container;
        private static AggregateCatalog _catalog;
        private static FileSystemWatcher _watcher;

        private static void Initialize()
        {
            try
            {
                _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Plugins");
                Directory.CreateDirectory(_path);
                _catalog = new AggregateCatalog();
                _catalog.Catalogs.Add(new DirectoryCatalog(path: _path, searchPattern: "*.dll"));
                _container = new CompositionContainer(_catalog, CompositionOptions.IsThreadSafe);

                FileLogger.Instance.Log(LogTypes.Info, "Catalog added to CompositionContainer.", $"Path: {_path}");
            }
            catch (Exception ex)
            {
                FileLogger.Instance.Log(LogTypes.Error, ex.Message, ex.StackTrace);

                throw;
            }
        }

        public static void Initialize(bool? isRecomposable = true)
        {
            Initialize();
            if (isRecomposable == true)
                StartWatch();
        }

        public static void Initialize<T>(T attributedPart, bool isRecomposable)
        {
            Initialize(isRecomposable);
            if (isRecomposable)
                ComposeParts(attributedPart);
            else
                lock (lockObj)
                    _container.SatisfyImportsOnce(attributedPart);
        }

        public static void ComposeParts(params object[] attributedParts)
        {
            lock (lockObj)
                _container.ComposeParts(attributedParts);
        }

        private static void StartWatch()
        {
            _watcher = new FileSystemWatcher(_path)
            {
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite |
                                    NotifyFilters.FileName | NotifyFilters.DirectoryName |
                                    NotifyFilters.Size | NotifyFilters.Security
            };
            _watcher.Changed += CheckFilesAndRefresh;
            _watcher.Created += CheckFilesAndRefresh;
            _watcher.Deleted += CheckFilesAndRefresh;
            _watcher.Renamed += CheckFilesAndRefresh;
            _watcher.Error += _watcher_Error;
            _watcher.EnableRaisingEvents = true;
        }

        private static void _watcher_Error(object sender, ErrorEventArgs e)
        {
            FileLogger.Instance.Log(LogTypes.Warning, "FileSystemWatcher event fired: ERROR", $"Error: {e}", $"Exception: {e.GetException().Message}");
        }

        private static void CheckFilesAndRefresh(object sender, FileSystemEventArgs e)
        {
            string lName = e.Name.ToLower();
            FileLogger.Instance.Log(LogTypes.Event, "FileSystemWatcher event fired.", $"File Name: {lName}", $"ChangeType: {e.ChangeType}");
            if (lName.EndsWith(".dll") || lName.EndsWith(".exe"))
                Refresh();
        }

        public static void Refresh()
        {
            foreach (DirectoryCatalog dCatalog in _catalog.Catalogs.ToList())
                dCatalog.Refresh();

            FileLogger.Instance.Log(LogTypes.Info, "Catalog Refreshed.");
        }
    }
}
