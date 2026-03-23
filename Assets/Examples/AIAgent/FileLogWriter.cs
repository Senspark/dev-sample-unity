using System;
using System.IO;
using UnityEngine;

namespace Examples.AIAgent
{
    public class FileLogWriter : IDisposable
    {
        readonly StreamWriter _writer;
        readonly object _lock = new();
        bool _disposed;

        public FileLogWriter(string filePath)
        {
            _writer = new StreamWriter(filePath, false) { AutoFlush = true };
            _writer.WriteLine("=== Editor Start Playing ===");
        }

        public void WriteLog(string condition, string stackTrace, LogType type)
        {
            if (_disposed) return;

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var line = $"[{timestamp}] [{type}] {condition}";

            lock (_lock)
            {
                if (_disposed) return;

                _writer.WriteLine(line);

                if (!string.IsNullOrEmpty(stackTrace) &&
                    type is LogType.Error or LogType.Exception or LogType.Assert)
                {
                    _writer.WriteLine(stackTrace);
                }
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                if (_disposed) return;
                _disposed = true;
                _writer.WriteLine("=== Editor Stop Playing ===");
                _writer.Flush();
                _writer.Close();
            }
        }
    }
}
