using StockQuoteAlertConsoleApp.Controllers;
using StockQuoteAlertConsoleApp.Utils;

namespace StockQuoteAlertConsoleApp.handlers;

public class MonitoringHandler
{
    private readonly MonitoringController _controller = new();
    private int CheckEveryIntervalMilliseconds => AppConfigUtils.CheckEveryIntervalSeconds!.Value * 1000;

    public async Task StartMonitoring(string[] args)
    {
        var request = _controller.ExtractRequestFromArgs(args);
        Console.WriteLine($"Start monitoring {request.TicketName}...");
        
        while (true)
        {
            await _controller.Execute(request);
            Thread.Sleep(CheckEveryIntervalMilliseconds);
        }
    }
}