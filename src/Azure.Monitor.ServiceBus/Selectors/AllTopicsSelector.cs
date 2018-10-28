using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Management;

namespace Azure.Monitor.ServiceBus.Selectors
{
    public class AllTopicsSelector : TopicSelector
    {
        private Func<TopicDescription, bool> _predicate;

        public AllTopicsSelector()
            : this(null)
        {

        }

        public AllTopicsSelector(Func<TopicDescription, bool> predicate)
        {
            _predicate = predicate ?? new Func<TopicDescription, bool>(t => true);
        }

        public override async Task<IEnumerable<string>> SelectTopicsAsync(ManagementClient client, TopicOptions options)
        {
            const int take = 100;
            int skip = 0;
            Queue<string> topics = new Queue<string>();
            bool hasMore = false;

            do
            {
                var topicDescriptions = await client.GetTopicsAsync(take, skip);

                foreach (var description in topicDescriptions.Where(d => _predicate(d)))
                {
                    topics.Enqueue(description.Path);
                }

                hasMore = topicDescriptions.Count == take;
            } while (hasMore);

            return topics;
        }
    }
}