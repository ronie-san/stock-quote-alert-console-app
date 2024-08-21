using StockQuoteAlertConsoleApp.models.Api;

namespace StockQuoteAlertConsoleApp.models.Facades;

public interface IStockQuoteFacade
{
    Task<StockQuoteResponse> GetStockQuote(string ticketName);
}