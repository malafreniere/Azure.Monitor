using System.Collections.Generic;
using System.IO;
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
            using (var ms = new MemoryStream())
            {
                _serializer.WriteObject(ms, record);

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public string Format(IEnumerable<MonitorRecord> records)
        {
            using (var ms = new MemoryStream())
            {
                _serializer.WriteObject(ms, records);

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}