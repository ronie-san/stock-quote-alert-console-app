using StockQuoteAlertConsoleApp.models.DTO;

namespace StockQuoteAlertConsoleApp.models.Adapters;

public interface IStockQuoteAdapter
{
    Task<StockQuoteDTO> GetStockQuote(string ticketName);
}