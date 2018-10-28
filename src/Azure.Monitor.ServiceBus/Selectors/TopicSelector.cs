using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Management;

namespace Azure.Monitor.ServiceBus.Selectors
{
    public class TopicSelector
    {
        private readonly IEnumerable<string> _topicNames;

        protected TopicSelector() { }

        public TopicSelector(string topicName)
         : this(new List<string> { topicName })
        {
        }

        public TopicSelector(IEnumerable<string> topicNames)
        {
            var topicsList = topicNames as List<string> ?? topicNames.ToList();
            _topicNames = topicsList.AsReadOnly();
        }

        public virtual Task<IEnumerable<string>> SelectTopicsAsync(ManagementClient client, TopicOptions options) => Task.FromResult(_topicNames);
    }
}