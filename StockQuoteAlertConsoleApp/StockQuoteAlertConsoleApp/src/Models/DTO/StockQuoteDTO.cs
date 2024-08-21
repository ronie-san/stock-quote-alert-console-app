using StockQuoteAlertConsoleApp.Enums;
using StockQuoteAlertConsoleApp.models.Api;

namespace StockQuoteAlertConsoleApp.models.DTO;

public class StockQuoteDTO
{
    public string? Symbol { get; set; }

    public double? Price { get; set; }

    public AlertEmailSuggestionEnum AlertEmailSuggestion { get; set; } = AlertEmailSuggestionEnum.NONE;

    public StockQuoteDTO(StockQuoteResponse stockQuoteResponse)
    {
        Symbol = stockQuoteResponse.GlobalQuote.Symbol;
        Price = double.TryParse(stockQuoteResponse.GlobalQuote.Price?.Replace('.', ','), out var price) ? price : null;
    }

    public StockQuoteDTO()
    {
    }
}