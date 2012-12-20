using System.Diagnostics;
using BuildMonitor.Service.Properties;

namespace BuildMonitor.Engine
{
    public class EventLogger : ILog
    {
        private readonly EventLog eventLog;

        public EventLogger(EventLog log, string source)
        {
            eventLog = log;
            eventLog.Source = source;
        }

        public void Information(string text)
        {
            WriteLog(text, EventLogEntryType.Information);
        }

        public void Warning(string text)
        {
            WriteLog(text, EventLogEntryType.Warning);
        }

        public void Error(string text)
        {
            WriteLog(text, EventLogEntryType.Error);
        }

        private void WriteLog(string text, EventLogEntryType type)
        {
            if ((!Settings.Default.LogEnabled) || (eventLog == null))
            {
                return;
            }

            eventLog.WriteEntry(text, type);
        }
    }
}