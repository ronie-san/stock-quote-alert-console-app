using System.Net.Mail;

namespace StockQuoteAlertConsoleApp.models.Adapters;

public interface ISmtpClientAdapter
{
    Task SendMailAsync(MailMessage message);
}