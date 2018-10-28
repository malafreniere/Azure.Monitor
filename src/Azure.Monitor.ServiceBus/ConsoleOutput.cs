using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Azure.Monitor.ServiceBus
{
    public class ResourceInfo
    {
        public string Namespace { get; set; }
        public string Path { get; set; }
        public long ActiveMessageCount { get; set; }
        public long DeadLetterMessageCount { get; set; }
        public long ScheduledMessageCount { get; set; }
        public long TransferMessageCount { get; set; }
        public long TransferDeadLetterMessageCount { get; set; }
        public long? SizeInBytes { get; set; }
    }

    public interface IOutput
    {
        Task OutputAsync(ResourceInfo info);
        Task FlushAsync();
    }

    public class ConsoleOutput : IOutput
    {
        public Task FlushAsync() => Task.CompletedTask;

        public Task OutputAsync(ResourceInfo info)
        {
            Console.WriteLine($"ServiceBus={info.Namespace}\tPath={info.Path}");
            Console.WriteLine($"\tActive={info.ActiveMessageCount}");
            Console.WriteLine($"\tScheduled={info.ScheduledMessageCount}");
            Console.WriteLine($"\tDeadLetter={info.DeadLetterMessageCount}");

            return Task.CompletedTask;
        }
    }

    public class AggregateOutput : IOutput
    {
        private readonly IList<IOutput> _outputs = new List<IOutput>();

        public void AddOutput(IOutput output) => _outputs.Add(output);

        public async Task FlushAsync()
        {
            foreach (var output in _outputs)
            {
                await output.FlushAsync();
            }
        }

        public async Task OutputAsync(ResourceInfo info)
        {
            foreach (var output in _outputs)
            {
                await output.OutputAsync(info);
            }
        }
    }

    public class FileOutput : IOutput
    {
        private readonly IFormatter _formatter;

        private readonly string _filePath;

        private Stack<string> _buffer = new Stack<string>();

        public FileOutput(string filePath, IFormatter formatter)
        {
            _filePath = filePath;
            _formatter = formatter;
        }

        public async Task FlushAsync()
        {
            using (var sw = new StreamWriter(File.OpenWrite(_filePath)))
            {
                while (_buffer.Any())
                {
                    await sw.WriteLineAsync(_buffer.Pop());
                }

                await sw.FlushAsync();
            }
        }

        public Task OutputAsync(ResourceInfo info)
        {
            _buffer.Push(_formatter.Format(info));

            return Task.CompletedTask;
        }
    }

    public interface IFormatter
    {
        string Format(ResourceInfo info);
    }

    public class JsonFormatter : IFormatter
    {
        public string Format(ResourceInfo info)
        {
            return JsonConvert.SerializeObject(info);
        }
    }
}