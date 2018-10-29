using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Monitor.Abstractions;

namespace Azure.Monitor.Outputs
{
    public class CompositeOutput : IBufferedMonitorOutput, IDisposable
    {
        private Stack<IMonitorOutput> _outputs = new Stack<IMonitorOutput>();

        public void AddOutput(IMonitorOutput output) => _outputs.Push(output);

        public void Dispose()
        {
            foreach (var output in _outputs.OfType<IDisposable>())
            {
                output.Dispose();
            }

            _outputs.Clear();
        }

        public async Task FlushAsync()
        {
            IList<Task> tasks = new List<Task>(_outputs.Count);

            foreach (var output in _outputs.OfType<IBufferedMonitorOutput>())
            {
                tasks.Add(output.FlushAsync());
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        public async Task OutputAsync(MonitorRecord record)
        {
            IList<Task> tasks = new List<Task>(_outputs.Count);

            foreach (var output in _outputs)
            {
                tasks.Add(output.OutputAsync(record));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}