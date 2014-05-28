using System;
using Movies.Interfaces;
using log4net;

namespace Movies.Utils
{
    public class Logger : ILogger
    {
        private readonly ILog _log;

        public Logger(string name = "MoviesService")
        {
            _log = LogManager.GetLogger(name ?? "");
        }

        public void Trace(string message)
        {
            _log.Debug(message);
        }

        public void LogInfo(string message)
        {
            _log.Info(message);
        }

        public void LogWarning(string message)
        {
            _log.Warn(message);
        }

        public void LogError(string message, Exception ex)
        {
            _log.Error(message, ex);
        }
    }
}