using System.Globalization;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using StockQuoteAlertConsoleApp.Adapters;
using StockQuoteAlertConsoleApp.models.Adapters;
using StockQuoteAlertConsoleApp.models.Api;
using StockQuoteAlertConsoleApp.models.Facades;

namespace StockQuoteAlertConsoleAppTests;

public class StockQuoteAdapterTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IStockQuoteFacade> _stockQuoteFacadeMock;

    private readonly IStockQuoteAdapter _stockQuoteAdapter;

    public StockQuoteAdapterTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _stockQuoteFacadeMock = _fixture.Freeze<Mock<IStockQuoteFacade>>();
        _stockQuoteAdapter = new StockQuoteAdapter(_stockQuoteFacadeMock.Object);
    }

    [Fact]
    private async Task TestIfThrowExceptionWhenFacadeThrows()
    {
        var mockedTicketName = _fixture.Create<string>();
        _stockQuoteFacadeMock.Setup(x => x.GetStockQuote(It.IsAny<string>())).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(async () => await _stockQuoteAdapter.GetStockQuote(mockedTicketName));
        _stockQuoteFacadeMock.Verify(x => x.GetStockQuote(mockedTicketName));
    }
    
    [Fact]
    private async Task TestIfReturnDtoSuccessfullyTicketNotFound()
    {
        var mockedTicketName = _fixture.Create<string>();
        var mockedResponse = new StockQuoteResponse
        {
            GlobalQuote = new GlobalQuoteResponse()
        };
        _stockQuoteFacadeMock.Setup(x => x.GetStockQuote(It.IsAny<string>())).ReturnsAsync(mockedResponse);
        
        var response = await _stockQuoteAdapter.GetStockQuote(mockedTicketName);
        _stockQuoteFacadeMock.Verify(x => x.GetStockQuote(mockedTicketName), Times.Once);
        Assert.NotNull(response);
        Assert.Equal(mockedResponse.GlobalQuote.Symbol, response.Symbol);
        Assert.Null(response.Price);
    }

    [Fact]
    private async Task TestIfReturnDtoSuccessfullyTicketFound()
    {
        var mockedTicketName = _fixture.Create<string>();
        var random = new Random();
        var mockedResponse = new StockQuoteResponse
        {
            GlobalQuote = new GlobalQuoteResponse
            {
                Symbol = mockedTicketName,
                Price = (random.Next() * random.NextDouble()).ToString(CultureInfo.InvariantCulture)
            }
        };
        _stockQuoteFacadeMock.Setup(x => x.GetStockQuote(It.IsAny<string>())).ReturnsAsync(mockedResponse);
        
        var response = await _stockQuoteAdapter.GetStockQuote(mockedTicketName);
        _stockQuoteFacadeMock.Verify(x => x.GetStockQuote(mockedTicketName), Times.Once);
        Assert.NotNull(response);
        Assert.Equal(mockedResponse.GlobalQuote.Symbol, response.Symbol);
        Assert.Equal(mockedResponse.GlobalQuote.Price, response.Price!.Value.ToString(CultureInfo.InvariantCulture));
    }
}