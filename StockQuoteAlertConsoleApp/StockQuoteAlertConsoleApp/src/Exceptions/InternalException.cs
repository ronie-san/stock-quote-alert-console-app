namespace StockQuoteAlertConsoleApp.exceptions;

public class InternalException(string message) : Exception($"Internal exception. {message}");