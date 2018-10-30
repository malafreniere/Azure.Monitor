using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Management;

namespace Azure.Monitor.ServiceBus.Selectors
{
    public class QueueSelector
    {
        private readonly IEnumerable<string> _queueNames;

        protected QueueSelector() { }

        public QueueSelector(string queueName)
         : this(new List<string> { queueName })
        {
        }

        public QueueSelector(IEnumerable<string> queueNames)
        {
            var queuesList = queueNames as List<string> ?? queueNames.ToList();
            _queueNames = queuesList.AsReadOnly();
        }

        public virtual Task<IEnumerable<string>> SelectQueuesAsync(ManagementClient client, QueueOptions options) => Task.FromResult(_queueNames);
    }
}