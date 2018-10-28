using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace Azure.Monitor.ServiceBus
{
    public class ServiceBusMonitor
    {
        private readonly Lazy<ManagementClient> _client;
        private readonly ServiceBusConnectionStringBuilder _builder;

        private ManagementClient Client { get => _client.Value; }

        private readonly AggregateOutput _output = new AggregateOutput();

        public ServiceBusMonitor(ServiceBusMonitorOptions options)
        {
            _builder = new ServiceBusConnectionStringBuilder(options.ConnectionString);
            _client = new Lazy<ManagementClient>(() => new ManagementClient(_builder));
        }

        public void AddOutput(IOutput output) => _output.AddOutput(output);

        public async Task MonitorTopicsAsync()
        {
            const int take = 100;
            int skip = 0;
            IList<TopicDescription> topics = null;

            do
            {
                topics = await Client.GetTopicsAsync(take, skip);

                foreach (var topic in topics)
                {
                    await MonitorTopicAsync(topic);
                }

                skip += take;
            } while (topics.Count == take);
        }

        public async Task MonitorTopicAsync(string topicPath)
        {
            var topic = await Client.GetTopicAsync(topicPath);

            await MonitorTopicAsync(topic);
        }

        private async Task MonitorTopicAsync(TopicDescription topic)
        {
            const int take = 100;
            int skip = 0;

            IList<SubscriptionDescription> subscriptions;

            do
            {
                subscriptions = await Client.GetSubscriptionsAsync(topic.Path, take, skip);

                foreach (var subscription in subscriptions)
                {
                    await MonitorSubscriptionAsync(topic, subscription.SubscriptionName, false);
                }

                skip += take;
            } while (subscriptions.Count == take);

            await _output.FlushAsync();
        }

        public async Task MonitorSubscriptionAsync(string topicPath, string subscriptionName)
        {
            var topic = await Client.GetTopicAsync(topicPath);

            await MonitorSubscriptionAsync(topic, subscriptionName, true);
        }

        private async Task MonitorSubscriptionAsync(TopicDescription topic, string subscriptionName, bool flush)
        {
            var subscription = await Client.GetSubscriptionRuntimeInfoAsync(topic.Path, subscriptionName);
            var topicInfo = await Client.GetTopicRuntimeInfoAsync(topic.Path);

            ResourceInfo info = new ResourceInfo
            {
                Path = $"{subscription.TopicPath}/{subscriptionName}",
                Namespace = $"{_builder.Endpoint}",
                ActiveMessageCount = subscription.MessageCountDetails.ActiveMessageCount,
                DeadLetterMessageCount = subscription.MessageCountDetails.DeadLetterMessageCount,
                ScheduledMessageCount = subscription.MessageCountDetails.ScheduledMessageCount,
                TransferMessageCount = subscription.MessageCountDetails.TransferMessageCount,
                TransferDeadLetterMessageCount = subscription.MessageCountDetails.TransferDeadLetterMessageCount
            };

            await _output.OutputAsync(info);

            if (flush)
            {
                await _output.FlushAsync();
            }
        }

        public async Task MonitorQueueAsync(string queuePath, bool flush = true)
        {
            var queue = await Client.GetQueueRuntimeInfoAsync(queuePath);

            ResourceInfo info = new ResourceInfo
            {
                Path = queuePath,
                Namespace = $"{_builder.EntityPath}",
                ActiveMessageCount = queue.MessageCountDetails.ActiveMessageCount,
                DeadLetterMessageCount = queue.MessageCountDetails.DeadLetterMessageCount,
                ScheduledMessageCount = queue.MessageCountDetails.ScheduledMessageCount,
                TransferMessageCount = queue.MessageCountDetails.TransferMessageCount,
                TransferDeadLetterMessageCount = queue.MessageCountDetails.TransferDeadLetterMessageCount,
                SizeInBytes = queue.SizeInBytes
            };

            await _output.OutputAsync(info);

            if (flush)
            {
                await _output.FlushAsync();
            }
        }

        public async Task MonitorQueuesAsync()
        {
            const int take = 100;
            int skip = 0;

            IList<QueueDescription> queues;

            do
            {
                queues = await Client.GetQueuesAsync(take, skip);

                foreach (var queue in queues)
                {
                    await MonitorQueueAsync(queue.Path);
                }

                skip += take;
            } while (queues.Count == take);

            await _output.FlushAsync();
        }
    }
}