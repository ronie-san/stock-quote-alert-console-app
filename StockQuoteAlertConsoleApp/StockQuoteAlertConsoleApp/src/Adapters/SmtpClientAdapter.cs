using System.Net.Mail;
using StockQuoteAlertConsoleApp.models.Adapters;
using StockQuoteAlertConsoleApp.Utils;

namespace StockQuoteAlertConsoleApp.Adapters;

public class SmtpClientAdapter(string host, int port) : ISmtpClientAdapter
{
    private readonly SmtpClient _smtpClient = new(host, port);

    public SmtpClientAdapter() : this(AppConfigUtils.SmtpServer!, AppConfigUtils.SmtpPort!.Value)
    {
    }

    public async Task SendMailAsync(MailMessage message)
    {
        await _smtpClient.SendMailAsync(message);
    }
}