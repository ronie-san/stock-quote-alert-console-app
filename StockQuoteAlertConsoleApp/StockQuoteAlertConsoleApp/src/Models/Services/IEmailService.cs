using StockQuoteAlertConsoleApp.Enums;

namespace StockQuoteAlertConsoleApp.models.Services;

public interface IEmailService
{
    Task SendAlertEmail(string ticketName, double price, AlertEmailSuggestionEnum alertEmailSuggestion);
}