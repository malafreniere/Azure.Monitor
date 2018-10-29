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
            const char indent = ' ';

            sb.AppendLine(record.Parent);

            sb.Append(indent);
            sb.AppendLine($"{record.ResourceType}: {record.ResourceName}");

            foreach (var property in record.Properties)
            {
                sb.Append(indent);
                sb.Append(indent);
                sb.AppendLine($"{property.Key}: {property.Value}");
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