namespace StockQuoteAlertConsoleApp.models;

public class MonitoringRequest
{
    public required string TicketName { get; set; }
    public double SaleRefPrice { get; set; }
    public double PurchaseRefPrice { get; set; }

    public void Deconstruct(
        out string ticketName,
        out double saleRefPrice,
        out double purchaseRefPrice)
    {
        ticketName = TicketName;
        saleRefPrice = SaleRefPrice;
        purchaseRefPrice = PurchaseRefPrice;
    }
}