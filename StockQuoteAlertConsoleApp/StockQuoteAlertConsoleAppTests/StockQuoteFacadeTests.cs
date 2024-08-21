using System.Net;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Moq.Protected;
using StockQuoteAlertConsoleApp.exceptions;
using StockQuoteAlertConsoleApp.Facades;
using StockQuoteAlertConsoleApp.models.Facades;

namespace StockQuoteAlertConsoleAppTests;

public class StockQuoteFacadeTests
{
    private readonly IFixture _fixture;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly IStockQuoteFacade _stockQuoteFacade;

    public StockQuoteFacadeTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _httpMessageHandlerMock = _fixture.Freeze<Mock<HttpMessageHandler>>();
        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);

        _stockQuoteFacade = new StockQuoteFacade(httpClient, "test");
    }

    [Fact]
    public async Task TestIfThrowInternalExceptionWhenGetAsync()
    {
        var mockedTicketName = _fixture.Create<string>();
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<InternalException>(async () =>
            await _stockQuoteFacade.GetStockQuote(mockedTicketName));
        _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }
    
    [Fact]
    public async Task TestIfThrowInternalExceptionWhenResponseDiffersFromModel()
    {
        var mockedTicketName = _fixture.Create<string>();
        var mockedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = _fixture.Create<StreamContent>()
        };
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockedResponse);

        await Assert.ThrowsAsync<InternalException>(async () =>
            await _stockQuoteFacade.GetStockQuote(mockedTicketName));
        _httpMessageHandlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }
}