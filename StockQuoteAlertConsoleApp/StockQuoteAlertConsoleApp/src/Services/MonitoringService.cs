using StockQuoteAlertConsoleApp.Adapters;
using StockQuoteAlertConsoleApp.Enums;
using StockQuoteAlertConsoleApp.exceptions;
using StockQuoteAlertConsoleApp.models.Adapters;
using StockQuoteAlertConsoleApp.models.DTO;
using StockQuoteAlertConsoleApp.models.Services;

namespace StockQuoteAlertConsoleApp.Services;

public class MonitoringService(IStockQuoteAdapter stockQuoteAdapter) : IMonitoringService
{
    public MonitoringService() : this(new StockQuoteAdapter())
    {
    }
    
    public async Task<StockQuoteDTO> CheckStockQuote(string ticketName, double saleRefPrice, double purchaseRefPrice)
    {
        var response = await stockQuoteAdapter.GetStockQuote(ticketName);

        if (response.Symbol == null)
        {
            throw new TicketNotFoundException();
        }

        if (response.Price > saleRefPrice)
        {
            response.AlertEmailSuggestion = AlertEmailSuggestionEnum.SALE;
        }
        else if (response.Price < purchaseRefPrice)
        {
            response.AlertEmailSuggestion = AlertEmailSuggestionEnum.PURCHASE;
        }

        return response;
    }
}