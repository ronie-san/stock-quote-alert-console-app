using StockQuoteAlertConsoleApp.models.DTO;

namespace StockQuoteAlertConsoleApp.models.Services;

public interface IMonitoringService
{
    Task<StockQuoteDTO> CheckStockQuote(string ticketName, double saleRefPrice, double purchaseRefPrice);
}