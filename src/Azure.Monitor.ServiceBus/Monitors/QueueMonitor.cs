using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Monitor.Abstractions;
using Microsoft.Azure.ServiceBus.Management;

namespace Azure.Monitor.ServiceBus
{
    internal class QueueMonitor : ServiceBusMonitorBase
    {
        private readonly QueueOptions _options;

        public QueueMonitor(QueueOptions options)
            : base(options.ConnectionString)
        {
            _options = options;
        }

        protected override async Task<IEnumerable<MonitorRecord>> MonitorAsync(string serviceBus, ManagementClient client)
        {
            var queuePaths = await _options.Selector.SelectQueuesAsync(client, _options);
            Queue<MonitorRecord> records = new Queue<MonitorRecord>();

            foreach (var queuePath in queuePaths)
            {
                var queue = await client.GetQueueRuntimeInfoAsync(queuePath);
                var record = new MonitorRecord(serviceBus, queue.Path, Properties.QueueResourceType).SetProperties(queue);

                records.Enqueue(record);
            }

            return records;
        }
    }
}