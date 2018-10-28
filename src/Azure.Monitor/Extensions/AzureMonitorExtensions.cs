using Azure.Monitor.Formatters;
using Azure.Monitor.Outputs;

namespace Azure.Monitor
{
    public static class AzureMonitorExtensions
    {
        public static AzureMonitor OutputJsonFile(this AzureMonitor monitor, string filePath)
        {
            monitor.AddOutput(new FileOutput(filePath, new JsonFormatter()));

            return monitor;
        }

        public static AzureMonitor OutputXmlFile(this AzureMonitor monitor, string filePath)
        {
            monitor.AddOutput(new FileOutput(filePath, new XmlFormatter()));

            return monitor;
        }

        public static AzureMonitor OutputConsole(this AzureMonitor monitor)
        {
            monitor.AddOutput(new ConsoleOutput(new StringFormatter()));

            return monitor;
        }

        public static AzureMonitor OutputJsonConsole(this AzureMonitor monitor)
        {
            monitor.AddOutput(new ConsoleOutput(new JsonFormatter()));

            return monitor;
        }

        public static AzureMonitor OutputXmlConsole(this AzureMonitor monitor)
        {
            monitor.AddOutput(new ConsoleOutput(new XmlFormatter()));

            return monitor;
        }
    }
}