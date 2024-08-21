using StockQuoteAlertConsoleApp.Constants;

namespace StockQuoteAlertConsoleApp.exceptions;

public class InvalidPriceValueException(int index)
    : InvalidArgsException(string.Format(ExceptionMessageConstants.INVALID_PRICE_VALUE, index));