using System.Text.Json.Serialization;

namespace StockQuoteAlertConsoleApp.models.Api;

public class StockQuoteResponse
{
    [JsonPropertyName("Global Quote")]
    public GlobalQuoteResponse GlobalQuote { get; set; }
}