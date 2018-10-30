using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Azure.Monitor.Abstractions;
using Azure.Monitor.Abstractions.Formatters;

namespace Azure.Monitor.Outputs
{
    public class FileOutput : BufferedMonitorOutputBase
    {
        private readonly string _filePath;
        private readonly IOutputFormatter _formatter;

        public FileOutput(string filePath, IOutputFormatter formatter)
        {
            _filePath = filePath;
            _formatter = formatter;
        }

        protected override async Task FlushAsync(IEnumerable<MonitorRecord> records)
        {
            using (var sw = new StreamWriter(File.Open(_filePath, FileMode.Create, FileAccess.Write)))
            {
                string output = _formatter.Format(records);

                await sw.WriteLineAsync(output).ConfigureAwait(false);
                await sw.FlushAsync().ConfigureAwait(false);
            }

            Debug.WriteLine($"Writing output to... {new FileInfo(_filePath).FullName}");
        }
    }
}