using System.Collections.Generic;
using System.Text;
using Azure.Monitor.Abstractions;
using Azure.Monitor.Abstractions.Formatters;

namespace Azure.Monitor.Formatters
{
    public class StringFormatter : IOutputFormatter
    {
        private void Format(StringBuilder sb, MonitorRecord record)
        {
            sb.AppendLine($"Name: {record.ResourceName} Type: {record.ResourceType} Parent: {record.Parent}");

            foreach (var property in record.Properties)
            {
                sb.AppendLine($"\t{property.Key}: {property.Value}");
            }
        }

        public string Format(MonitorRecord record)
        {
            StringBuilder sb = new StringBuilder();

            Format(sb, record);

            return sb.ToString();
        }

        public string Format(IEnumerable<MonitorRecord> records)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var record in records)
            {
                Format(sb, record);
            }

            return sb.ToString();
        }
    }
}