using System.Collections.Generic;

namespace Azure.Monitor.Abstractions.Formatters
{
    public interface IOutputFormatter
    {
        string Format(MonitorRecord record);

        string Format(IEnumerable<MonitorRecord> records);
    }
}