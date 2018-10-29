using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Monitor.Abstractions;
using Microsoft.Azure.ServiceBus.Management;

namespace Azure.Monitor.ServiceBus
{
    internal class TopicMonitor : ServiceBusMonitorBase
    {
        private readonly TopicOptions _options;

        internal TopicMonitor(TopicOptions options)
            : base(options.ConnectionString)
        {
            _options = options;
        }

        protected override async Task<IEnumerable<MonitorRecord>> MonitorAsync(string serviceBus, ManagementClient client)
        {
            var topicPaths = await _options.Selector.SelectTopicsAsync(client, _options).ConfigureAwait(false);
            Queue<MonitorRecord> records = new Queue<MonitorRecord>();

            foreach (var topicPath in topicPaths)
            {
                var topic = await client.GetTopicRuntimeInfoAsync(topicPath).ConfigureAwait(false);
                var record = new MonitorRecord(serviceBus, topic.Path, Properties.TopicResourceType).SetProperties(topic);

                records.Enqueue(record);
            }

            return records;
        }
    }
}