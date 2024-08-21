namespace StockQuoteAlertConsoleApp.Enums;

public enum AlertEmailSuggestionEnum
{
    SALE,
    PURCHASE,
    NONE
}

public static class AlertEmailSuggestionEnumMethods
{
    public static string ToFriendlyName(this AlertEmailSuggestionEnum alertEmailSuggestion)
    {
        switch (alertEmailSuggestion)
        {
            case AlertEmailSuggestionEnum.SALE:
                return "SALE";
            case AlertEmailSuggestionEnum.PURCHASE:
                return "PURCHASE";
            // NOTE: We don't need to worry about other cases since the alert e-mail will only be sent for above cases.
            default:
                return "";
        }
    }
}