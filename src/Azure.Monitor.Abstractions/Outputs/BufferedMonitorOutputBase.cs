using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Azure.Monitor.Abstractions
{
    public abstract class BufferedMonitorOutputBase : IBufferedMonitorOutput
    {
        protected ConcurrentQueue<MonitorRecord> Records = new ConcurrentQueue<MonitorRecord>();

        public async Task FlushAsync()
        {
            await FlushAsync(Records).ConfigureAwait(false);

            Records = new ConcurrentQueue<MonitorRecord>();
        }

        protected abstract Task FlushAsync(IEnumerable<MonitorRecord> records);

        public Task OutputAsync(MonitorRecord record)
        {
            Records.Enqueue(record);

            return Task.CompletedTask;
        }
    }
}