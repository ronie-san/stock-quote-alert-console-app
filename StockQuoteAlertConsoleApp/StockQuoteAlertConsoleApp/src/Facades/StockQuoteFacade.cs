using System.Text.Json;
using StockQuoteAlertConsoleApp.exceptions;
using StockQuoteAlertConsoleApp.models.Api;
using StockQuoteAlertConsoleApp.models.Facades;
using StockQuoteAlertConsoleApp.Utils;

namespace StockQuoteAlertConsoleApp.Facades;

public class StockQuoteFacade(HttpClient httpClient, string apiKey) : IStockQuoteFacade
{
    private const string BASE_URL = "https://www.alphavantage.co";

    public StockQuoteFacade() : this(new HttpClient(), AppConfigUtils.ApiKey!)
    {
    }

    public async Task<StockQuoteResponse> GetStockQuote(string ticketName)
    {
        httpClient.DefaultRequestHeaders.Accept.Clear();
        var url = MountUrlRequest(ticketName);
        StockQuoteResponse? deserializedResponse;
        
        try
        {
            var response = await httpClient.GetStreamAsync(url);
            deserializedResponse = await JsonSerializer.DeserializeAsync<StockQuoteResponse>(response);
        }
        catch (Exception e)
        {
            throw new InternalException(e.Message);
        }
        
        if (deserializedResponse == null)
        {
            throw new InternalException("There was no response from the Stock Quote API");
        }
        
        return deserializedResponse;
    }

    private string MountUrlRequest(string ticketName)
    {
        var url = $"{BASE_URL}/query?function=GLOBAL_QUOTE&symbol={ticketName}&apikey={apiKey}";
        return new Uri(url).ToString();
    }
}