using Azure.Monitor.ServiceBus.Selectors;

namespace Azure.Monitor.ServiceBus
{
    public class QueueOptions : ServiceBusOptions
    {
        public QueueSelector Selector { get; set; } = new AllQueuesSelector();
    }
}