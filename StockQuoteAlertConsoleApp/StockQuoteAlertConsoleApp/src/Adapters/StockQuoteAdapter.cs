using StockQuoteAlertConsoleApp.Facades;
using StockQuoteAlertConsoleApp.models.Adapters;
using StockQuoteAlertConsoleApp.models.DTO;
using StockQuoteAlertConsoleApp.models.Facades;

namespace StockQuoteAlertConsoleApp.Adapters;

public class StockQuoteAdapter(IStockQuoteFacade stockQuoteFacade) : IStockQuoteAdapter
{
    public StockQuoteAdapter() : this(new StockQuoteFacade())
    {
    }

    public async Task<StockQuoteDTO> GetStockQuote(string ticketName)
    {
        var response = await stockQuoteFacade.GetStockQuote(ticketName);
        return new StockQuoteDTO(response);
    }
}