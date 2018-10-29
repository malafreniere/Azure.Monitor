using System;
using Azure.Monitor.Abstractions;
using Microsoft.Azure.ServiceBus.Management;

namespace Azure.Monitor.ServiceBus
{
    internal static class MonitorRecordExtensions
    {
        private static MonitorRecord SetActiveMessageCount(this MonitorRecord record, long count)
            => record.Set(Properties.ActiveMessageCount, count);

        private static MonitorRecord SetDeadLetterMessageCount(this MonitorRecord record, long count)
            => record.Set(Properties.DeadLetterMessageCount, count);

        private static MonitorRecord SetScheduledMessageCount(this MonitorRecord record, long count)
            => record.Set(Properties.ScheduledMessageCount, count);

        private static MonitorRecord SetSizeInBytes(this MonitorRecord record, long size)
            => record.Set(Properties.SizeInBytes, size);

        private static MonitorRecord SetLastAccessedAt(this MonitorRecord record, DateTime value)
            => record.Set(Properties.LastAccessedAt, value.ToString());

        private static MonitorRecord SetSubscriptionCount(this MonitorRecord record, long count)
            => record.Set(Properties.SubscriptionCount, count);

        private static MonitorRecord Set<T>(this MonitorRecord record, string key, T value)
        {
            record[key] = value;

            return record;
        }

        private static MonitorRecord SetCountDetails(this MonitorRecord record, MessageCountDetails details)
            => record.SetActiveMessageCount(details.ActiveMessageCount)
                     .SetDeadLetterMessageCount(details.DeadLetterMessageCount)
                     .SetScheduledMessageCount(details.ScheduledMessageCount);
        internal static MonitorRecord SetProperties(this MonitorRecord record, TopicRuntimeInfo topic)
            => record.SetCountDetails(topic.MessageCountDetails)
                     .SetSizeInBytes(topic.SizeInBytes)
                     .SetSubscriptionCount(topic.SubscriptionCount)
                     .SetLastAccessedAt(topic.AccessedAt);

        internal static MonitorRecord SetProperties(this MonitorRecord record, QueueRuntimeInfo queue)
            => record.SetCountDetails(queue.MessageCountDetails)
                     .SetSizeInBytes(queue.SizeInBytes)
                     .SetLastAccessedAt(queue.AccessedAt);

    }
}