using System.Net.Mail;
using System.Text;
using StockQuoteAlertConsoleApp.Adapters;
using StockQuoteAlertConsoleApp.Enums;
using StockQuoteAlertConsoleApp.exceptions;
using StockQuoteAlertConsoleApp.models.Adapters;
using StockQuoteAlertConsoleApp.models.Services;
using StockQuoteAlertConsoleApp.Utils;

namespace StockQuoteAlertConsoleApp.Services;

public class EmailService(
    ISmtpClientAdapter smtpClientAdapter,
    string alertEmailSender,
    string alertEmailDestination)
    : IEmailService
{
    public EmailService() : this(new SmtpClientAdapter(), AppConfigUtils.AlertEmailSender!,
        AppConfigUtils.AlertEmailDestination!)
    {
    }

    public async Task SendAlertEmail(string ticketName, double price, AlertEmailSuggestionEnum alertEmailSuggestion)
    {
        try
        {
            using var mailMessage = MountMailMessage(ticketName, price, alertEmailSuggestion);
            await smtpClientAdapter.SendMailAsync(mailMessage);
        }
        catch (Exception e)
        {
            throw new InternalException(e.Message);
        }
    }

    private MailMessage MountMailMessage(string ticketName, double price, AlertEmailSuggestionEnum alertEmailSuggestion)
    {
        var mailAddressFrom = new MailAddress(alertEmailSender);
        var mailAddressTo = new MailAddress(alertEmailDestination);

        var mailMessage = new MailMessage(mailAddressFrom, mailAddressTo);
        mailMessage.Body =
            $"The monitored ticket reached the price of {price}. You should {alertEmailSuggestion.ToFriendlyName()} it.";
        mailMessage.BodyEncoding = Encoding.UTF8;
        mailMessage.Subject = $"Alert e-mail for {alertEmailSuggestion.ToFriendlyName()}. Ticket {ticketName}";
        mailMessage.SubjectEncoding = Encoding.UTF8;

        return mailMessage;
    }
}