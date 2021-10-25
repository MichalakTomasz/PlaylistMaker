using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMaker.Services
{
    public class LogService : ILogService
    {
        public bool IsLoggerEnabled { get; set; }

        public void LogError(Exception e)
        {

        }
        public void LogError(string message)
        {

        }

        public void LogMessage(string message)
        {

        }

        public void LogDebug(string message)
        {

        }
    }
}
