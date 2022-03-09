using System;
using System.IO;
using System.Text;

namespace Streaming.BusinessLogic
{
    public enum LogTypes : byte
    {
        Error,
        Warning,
        Info,
        Event
    }

    public class FileLogger
    {
        private static object _lock = new object();
        private static FileLogger _instance = null;

        static FileLogger() { }
        public FileLogger() { }

        public static FileLogger Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new FileLogger();
                    }
                }
                return _instance;
            }
        }

        public void Log(LogTypes logTypes, params string[] messages)
        {
            var root = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fileFullPath = Path.Combine(root, "Log", $"Log_{DateTime.UtcNow:yyyy-MM-dd}.txt");
            if (!File.Exists(fileFullPath))
            {
                Directory.CreateDirectory(Path.Combine(root, "Log"));
                using var file = File.Create(fileFullPath);
            }

            var _builder = new StringBuilder();
            _builder.Clear();
            _builder.AppendLine();
            _builder.AppendLine(logTypes.ToString());
            _builder.AppendLine(DateTime.Now.ToString());
            for (int i = 0; i < messages.Length; i++)
            {
                _builder.AppendLine(messages[i]);
            }
            File.AppendAllText(fileFullPath, _builder.ToString());
        }
    }
}
