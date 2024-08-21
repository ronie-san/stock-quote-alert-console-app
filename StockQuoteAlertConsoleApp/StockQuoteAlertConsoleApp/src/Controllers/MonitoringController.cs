using StockQuoteAlertConsoleApp.Enums;
using StockQuoteAlertConsoleApp.exceptions;
using StockQuoteAlertConsoleApp.models;
using StockQuoteAlertConsoleApp.models.Services;
using StockQuoteAlertConsoleApp.Services;

namespace StockQuoteAlertConsoleApp.Controllers;

public class MonitoringController(IMonitoringService monitoringService, IEmailService emailService)
{
    public MonitoringController() : this(new MonitoringService(), new EmailService())
    {
    }

    public MonitoringRequest ExtractRequestFromArgs(string[] args)
    {
        if (args.Length != 3)
        {
            throw new InvalidArgsLengthException(args.Length);
        }

        var tickerName = args[0];
        var (saleRefPrice, purchaseRefPrice) = ValidatePriceArgs(args[1].Replace('.',','), args[2].Replace('.',','));

        return new MonitoringRequest
        {
            TicketName = tickerName,
            PurchaseRefPrice = purchaseRefPrice,
            SaleRefPrice = saleRefPrice
        };
    }

    public async Task Execute(MonitoringRequest request)
    {
        var (ticketName, saleRefPrice, purchaseRefPrice) = request;
        var response = await monitoringService.CheckStockQuote(ticketName, saleRefPrice, purchaseRefPrice);

        if (response.AlertEmailSuggestion != AlertEmailSuggestionEnum.NONE)
        {
            await emailService.SendAlertEmail(ticketName, response.Price!.Value,  response.AlertEmailSuggestion);
            Console.WriteLine("Alert e-mail sent!");
        }
    }

    private (double, double) ValidatePriceArgs(string arg1, string arg2)
    {
        if (!double.TryParse(arg1, out var saleRefPrice))
        {
            throw new InvalidArgTypeException(arg1, 1, "double");
        }

        if (saleRefPrice < 0)
        {
            throw new InvalidPriceValueException(1);
        }

        if (!double.TryParse(arg2, out var purchaseRefPrice))
        {
            throw new InvalidArgTypeException(arg2, 2, "double");
        }
        
        if (purchaseRefPrice < 0)
        {
            throw new InvalidPriceValueException(2);
        }

        if (saleRefPrice < purchaseRefPrice)
        {
            throw new InvalidPriceRangeException();
        }

        return (saleRefPrice, purchaseRefPrice);
    }
}