using System.Runtime.Serialization;
using Azure.Monitor.Abstractions;
using Azure.Monitor.Abstractions.Formatters;

namespace Azure.Monitor.Formatters
{
    public class XmlFormatter : DataContractFormatterBase<DataContractSerializer>
    {
    }
}