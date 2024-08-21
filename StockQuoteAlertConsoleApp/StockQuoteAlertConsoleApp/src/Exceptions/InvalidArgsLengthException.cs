using StockQuoteAlertConsoleApp.Constants;

namespace StockQuoteAlertConsoleApp.exceptions;

public class InvalidArgsLengthException(int actualLength) : InvalidArgsException(string.Format(ExceptionMessageConstants.INVALID_ARGS_LENGTH, actualLength));