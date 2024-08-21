using StockQuoteAlertConsoleApp.Constants;

namespace StockQuoteAlertConsoleApp.exceptions;

public class InvalidArgTypeException(string arg, int index, string expectedType)
    : InvalidArgsException(string.Format(ExceptionMessageConstants.INVALID_ARG_TYPE, arg, index, expectedType));