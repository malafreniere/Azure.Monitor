using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Azure.Monitor.Abstractions;
using Azure.Monitor.Abstractions.Formatters;

namespace Azure.Monitor.Formatters
{
    public class JsonFormatter : DataContractFormatterBase
    {
        public JsonFormatter()
            : base(new DataContractJsonSerializer(typeof(MonitorRecord)))
        {
        }
    }
}