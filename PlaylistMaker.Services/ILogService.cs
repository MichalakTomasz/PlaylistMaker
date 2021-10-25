using System;

namespace PlaylistMaker.Services
{
    public interface ILogService
    {
        bool IsLoggerEnabled { get; set; }

        void LogDebug(string message);
        void LogError(Exception e);
        void LogError(string message);
        void LogMessage(string message);
    }
}