using System;

namespace Movies.Interfaces
{
    public interface ILogger
    {
        void Trace(string message);
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message, Exception ex);
    }
}