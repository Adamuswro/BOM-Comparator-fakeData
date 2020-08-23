using System;

namespace BOMComparator.Core.Models
{
    public class LogEntry
    {
        public LogEntry(string message)
        {
            Message = message;
            TimeStamp = DateTime.Now;
        }

        public string Message { get; private set; }
        public DateTime TimeStamp { get; private set; }
    }
}
