using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Monitor.Abstractions;

namespace Azure.Monitor.Outputs
{
    public class InMemoryOutput : IMonitorOutput
    {
        private ConcurrentQueue<MonitorRecord> _records = new ConcurrentQueue<MonitorRecord>();

        public IEnumerable<MonitorRecord> Records => _records;

        public Task OutputAsync(MonitorRecord record)
        {
            _records.Enqueue(record);

            return Task.CompletedTask;
        }
    }
}