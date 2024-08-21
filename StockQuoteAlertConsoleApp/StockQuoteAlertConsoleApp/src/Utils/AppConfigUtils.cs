using System.Configuration;

namespace StockQuoteAlertConsoleApp.Utils;

public static class AppConfigUtils
{
    public static string? AlertEmailDestination => ConfigurationManager.AppSettings["AlertEmailDestination"];
    public static string? SmtpServer => ConfigurationManager.AppSettings["SMTPServer"];
    public static string? ApiKey => ConfigurationManager.AppSettings["APIKey"];
    public static string? AlertEmailSender => ConfigurationManager.AppSettings["AlertEmailSender"];
    public static int? SmtpPort
    {
        get
        {
            var value = ConfigurationManager.AppSettings["SMTPPort"];
            return int.TryParse(value, out var port) ? port : null;
        }
    }

    public static int? CheckEveryIntervalSeconds
    {
        get
        {
            var value = ConfigurationManager.AppSettings["CheckEveryIntervalSeconds"];
            return int.TryParse(value, out var interval) ? interval : null;
        }
    }
}