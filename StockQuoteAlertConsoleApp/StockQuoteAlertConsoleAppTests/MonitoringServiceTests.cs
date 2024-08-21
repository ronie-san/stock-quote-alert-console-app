using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using StockQuoteAlertConsoleApp.Enums;
using StockQuoteAlertConsoleApp.exceptions;
using StockQuoteAlertConsoleApp.models.Adapters;
using StockQuoteAlertConsoleApp.models.DTO;
using StockQuoteAlertConsoleApp.models.Services;
using StockQuoteAlertConsoleApp.Services;

namespace StockQuoteAlertConsoleAppTests;

public class MonitoringServiceTests
{
    private readonly IFixture _fixture;

    private readonly Mock<IStockQuoteAdapter> _stockQuoteAdapterMock;
    private readonly IMonitoringService _monitoringService;

    public MonitoringServiceTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _stockQuoteAdapterMock = _fixture.Freeze<Mock<IStockQuoteAdapter>>();
        _monitoringService = new MonitoringService(_stockQuoteAdapterMock.Object);
    }

    [Fact]
    private async Task TestIfReturnSuccessfullyNoSuggestion()
    {
        var mockTicketName = _fixture.Create<string>();
        var mockSaleRefPrice = double.Abs(_fixture.Create<double>());
        var mockPurchaseRefPrice = GetRandomDouble(0, mockSaleRefPrice);
        var mockResponsePrice = GetRandomDouble(mockPurchaseRefPrice, mockSaleRefPrice);

        var mockResponse = new StockQuoteDTO
        {
            Symbol = mockTicketName,
            Price = mockResponsePrice
        };
        _stockQuoteAdapterMock.Setup(x => x.GetStockQuote(It.IsAny<string>())).ReturnsAsync(mockResponse);

        StockQuoteDTO? response = null;
        var exception = await Record.ExceptionAsync(async () =>
            response = await _monitoringService.CheckStockQuote(mockTicketName, mockSaleRefPrice,
                mockPurchaseRefPrice));
        _stockQuoteAdapterMock.Verify(x => x.GetStockQuote(mockTicketName), Times.Once);
        Assert.Null(exception);
        Assert.NotNull(response);
        Assert.Equal(AlertEmailSuggestionEnum.NONE, response.AlertEmailSuggestion);
    }

    [Fact]
    private async Task TestIfReturnSuccessfullySaleSuggestion()
    {
        var mockTicketName = _fixture.Create<string>();
        var mockSaleRefPrice = double.Abs(_fixture.Create<double>());
        var mockPurchaseRefPrice = GetRandomDouble(0, mockSaleRefPrice);
        var mockResponsePrice = GetRandomDouble(mockSaleRefPrice);

        var mockResponse = new StockQuoteDTO
        {
            Symbol = mockTicketName,
            Price = mockResponsePrice
        };
        _stockQuoteAdapterMock.Setup(x => x.GetStockQuote(It.IsAny<string>())).ReturnsAsync(mockResponse);

        StockQuoteDTO? response = null;
        var exception = await Record.ExceptionAsync(async () =>
            response = await _monitoringService.CheckStockQuote(mockTicketName, mockSaleRefPrice,
                mockPurchaseRefPrice));
        _stockQuoteAdapterMock.Verify(x => x.GetStockQuote(mockTicketName), Times.Once);
        Assert.Null(exception);
        Assert.NotNull(response);
        Assert.Equal(AlertEmailSuggestionEnum.SALE, response.AlertEmailSuggestion);
    }

    [Fact]
    private async Task TestIfReturnSuccessfullyPurchaseSuggestion()
    {
        var mockTicketName = _fixture.Create<string>();
        var mockSaleRefPrice = double.Abs(_fixture.Create<double>());
        var mockPurchaseRefPrice = GetRandomDouble(0, mockSaleRefPrice);
        var mockResponsePrice = GetRandomDouble(0, mockPurchaseRefPrice);

        var mockResponse = new StockQuoteDTO
        {
            Symbol = mockTicketName,
            Price = mockResponsePrice
        };
        _stockQuoteAdapterMock.Setup(x => x.GetStockQuote(It.IsAny<string>())).ReturnsAsync(mockResponse);

        StockQuoteDTO? response = null;
        var exception = await Record.ExceptionAsync(async () =>
            response = await _monitoringService.CheckStockQuote(mockTicketName, mockSaleRefPrice,
                mockPurchaseRefPrice));
        _stockQuoteAdapterMock.Verify(x => x.GetStockQuote(It.IsAny<string>()), Times.Once);
        Assert.Null(exception);
        Assert.NotNull(response);
        Assert.Equal(AlertEmailSuggestionEnum.PURCHASE, response.AlertEmailSuggestion);
    }

    [Fact]
    private async Task TestIfThrowExceptionWhenTicketNotFound()
    {
        var mockTicketName = _fixture.Create<string>();
        var mockSaleRefPrice = double.Abs(_fixture.Create<double>());
        var mockPurchaseRefPrice = GetRandomDouble(0, mockSaleRefPrice);

        var mockResponse = new StockQuoteDTO
        {
            Symbol = null
        };
        _stockQuoteAdapterMock.Setup(x => x.GetStockQuote(It.IsAny<string>())).ReturnsAsync(mockResponse);

        await Assert.ThrowsAsync<TicketNotFoundException>(async () =>
            await _monitoringService.CheckStockQuote(mockTicketName, mockSaleRefPrice, mockPurchaseRefPrice));
        _stockQuoteAdapterMock.Verify(x => x.GetStockQuote(mockTicketName), Times.Once);
    }

    private static double GetRandomDouble(double min, double max)
    {
        return new Random().NextDouble() * double.Abs(max - min) + double.Min(max, min);
    }

    private static double GetRandomDouble(double min)
    {
        var random = new Random();
        return min + random.Next() * random.NextDouble();
    }
}