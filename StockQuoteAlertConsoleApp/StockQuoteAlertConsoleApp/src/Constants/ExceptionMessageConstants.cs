namespace StockQuoteAlertConsoleApp.Constants;

public static class ExceptionMessageConstants
{
    public const string INVALID_ARGS_LENGTH = "Invalid number of args. Expected 3, actual {0}";
    public const string INVALID_ARG_TYPE = "Invalid arg '{0}' at position {1}. Expected {2}";
    public const string INVALID_PRICE_ARGS_RANGE = "Invalid range values. Value at position 1 must be greater than value at position 2";
    public const string INVALID_PRICE_VALUE = "Invalid value ate position {0}. It must be a positive value";
}