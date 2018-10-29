using System;
using System.Collections.Generic;
using System.Linq;

namespace Azure.Monitor.ServiceBus
{
    public class ServiceBusMonitorOptions
    {
        internal bool MonitorTopics { get; set; } = false;
        internal bool MonitorQueues { get; set; } = false;
        internal string[] TopicsToMonitor { get; set; } = null;
        internal string[] QueuesToMonitor { get; set; } = null;


        public ServiceBusMonitorOptions AllTopics()
        {
            MonitorTopics = true;

            return this;
        }

        public ServiceBusMonitorOptions Topics(params string[] topics)
        {
            MonitorTopics = true;

            TopicsToMonitor = topics;

            return this;
        }

        public ServiceBusMonitorOptions AllQueues()
        {
            MonitorQueues = true;
            return this;
        }

        public ServiceBusMonitorOptions Queues(params string[] queues)
        {
            MonitorQueues = true;

            QueuesToMonitor = queues;

            return this;
        }
    }

    public static class AzureMonitorExtensions
    {
        public static AzureMonitor ForServiceBus(this AzureMonitor monitor, string connectionString, Action<ServiceBusMonitorOptions> setup)
        {
            ServiceBusMonitorOptions opts = new ServiceBusMonitorOptions();

            setup(opts);

            return monitor.MonitorTopicsIf(connectionString, opts.MonitorTopics, opts.TopicsToMonitor)
                          .MonitorQueuesIf(connectionString, opts.MonitorQueues, opts.QueuesToMonitor);
        }

        private static AzureMonitor MonitorTopicsIf(this AzureMonitor monitor, string connectionString, bool monitorTopics, IEnumerable<string> topics)
        {
            if (!monitorTopics) return monitor;

            TopicOptions options = new TopicOptions
            {
                ConnectionString = connectionString
            };

            options.Selector = topics == null ? options.Selector : new Selectors.TopicSelector(topics);

            monitor.AddMonitor(new TopicMonitor(options));

            return monitor;
        }

        private static AzureMonitor MonitorQueuesIf(this AzureMonitor monitor, string connectionString, bool monitorQueues, IEnumerable<string> queues)
        {
            if (!monitorQueues) return monitor;

            QueueOptions options = new QueueOptions
            {
                ConnectionString = connectionString
            };

            options.Selector = queues == null ? options.Selector : new Selectors.QueueSelector(queues);

            monitor.AddMonitor(new QueueMonitor(options));

            return monitor;
        }
    }
}