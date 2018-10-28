using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Monitor.Abstractions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace Azure.Monitor.ServiceBus
{
    internal abstract class ServiceBusMonitorBase : IResourceMonitor
    {
        private readonly Lazy<ManagementClient> _client;
        private readonly ServiceBusConnectionStringBuilder _builder;
        private ManagementClient Client { get => _client.Value; }
        private readonly string _namespace;

        internal ServiceBusMonitorBase(string connectionString)
        {
            _builder = new ServiceBusConnectionStringBuilder(connectionString);
            _client = new Lazy<ManagementClient>(() => new ManagementClient(_builder));
            _namespace = _builder.Endpoint.Replace("sb://", string.Empty);
        }

        public async Task MonitorAsync(IMonitorOutput output)
        {
            var records = await MonitorAsync(_namespace, Client);

            foreach (var record in records)
            {
                await output.OutputAsync(record);
            }
        }

        protected abstract Task<IEnumerable<MonitorRecord>> MonitorAsync(string serviceBus, ManagementClient client);
    }
}