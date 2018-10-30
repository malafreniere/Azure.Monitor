using Azure.Monitor.ServiceBus.Selectors;

namespace Azure.Monitor.ServiceBus
{
    public class TopicOptions : ServiceBusOptions
    {
        public TopicSelector Selector { get; set; } = new AllTopicsSelector();
    }
}