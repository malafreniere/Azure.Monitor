using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Management;

namespace Azure.Monitor.ServiceBus.Selectors
{
    public class AllQueuesSelector : QueueSelector
    {
        private Func<QueueDescription, bool> _predicate;

        public AllQueuesSelector()
            : this(null)
        {
        }

        public AllQueuesSelector(Func<QueueDescription, bool> predicate)
        {
            _predicate = predicate ?? new Func<QueueDescription, bool>(t => true);
        }

        public override async Task<IEnumerable<string>> SelectQueuesAsync(ManagementClient client, QueueOptions options)
        {
            const int take = 100;
            int skip = 0;
            Queue<string> queues = new Queue<string>();
            bool hasMore = false;

            do
            {
                var queueDescriptions = await client.GetQueuesAsync(take, skip);

                foreach (var description in queueDescriptions.Where(d => _predicate(d)))
                {
                    queues.Enqueue(description.Path);
                }

                hasMore = queueDescriptions.Count == take;
            } while (hasMore);

            return queues;
        }
    }
}