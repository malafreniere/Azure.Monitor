using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Azure.Monitor.Abstractions;
using Azure.Monitor.Abstractions.Formatters;

namespace Azure.Monitor.Formatters
{
    public abstract class DataContractFormatterBase : IOutputFormatter
    {
        protected readonly XmlObjectSerializer _serializer;

        protected DataContractFormatterBase(XmlObjectSerializer serializer)
        {
            _serializer = serializer;
        }

        public string Format(MonitorRecord record)
        {
            return Format(new MonitorRecords
            {
                Records = new[] { record }
            });
        }

        public string Format(IEnumerable<MonitorRecord> records)
        {
            return Format(new MonitorRecords
            {
                Records = records.ToArray()
            });
        }

        private string Format(MonitorRecords records)
        {
            using (var ms = new MemoryStream())
            {
                _serializer.WriteObject(ms, records);

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}