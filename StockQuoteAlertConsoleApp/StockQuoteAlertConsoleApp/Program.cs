using StockQuoteAlertConsoleApp.handlers;

namespace StockQuoteAlertConsoleApp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var handler = new MonitoringHandler();
            try
            {
                await handler.StartMonitoring(args);
            }
            catch (Exception e)
            {
                await Console.Error.WriteLineAsync(e.Message);
            }
        }
    }
}