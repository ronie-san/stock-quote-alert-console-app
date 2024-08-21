using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using StockQuoteAlertConsoleApp.Controllers;
using StockQuoteAlertConsoleApp.Enums;
using StockQuoteAlertConsoleApp.exceptions;
using StockQuoteAlertConsoleApp.models;
using StockQuoteAlertConsoleApp.models.DTO;
using StockQuoteAlertConsoleApp.models.Services;

namespace StockQuoteAlertConsoleAppTests;

public class MonitoringControllerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IMonitoringService> _monitoringServiceMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly MonitoringController _controller;

    public MonitoringControllerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _monitoringServiceMock = _fixture.Freeze<Mock<IMonitoringService>>();
        _emailServiceMock = _fixture.Freeze<Mock<IEmailService>>();
        _controller = new MonitoringController(_monitoringServiceMock.Object, _emailServiceMock.Object);
    }

    [Theory]
    [InlineData]
    [InlineData("X")]
    [InlineData("X", "0")]
    [InlineData("X", "a", "a", "a")]
    private void TestIfThrowExceptionWhenInvalidArgsLength(params string[] args)
    {
        Assert.Throws<InvalidArgsLengthException>(() => _controller.ExtractRequestFromArgs(args));
    }

    [Theory]
    [InlineData("test", "30", "test")]
    [InlineData("test", "test", "40;9")]
    [InlineData("test", "test", "test")]
    private void TestIfThrowExceptionWhenInvalidPriceTypeArgs(params string[] args)
    {
        Assert.Throws<InvalidArgTypeException>(() => _controller.ExtractRequestFromArgs(args));
    }

    [Theory]
    [InlineData("test", "-20.7154", "0")]
    [InlineData("test", "23,164", "-10.4658")]
    [InlineData("test", "-1,304", "-5,1222")]
    private void TestIfThrowExceptionWhenInvalidPriceValueArgs(params string[] args)
    {
        Assert.Throws<InvalidPriceValueException>(() => _controller.ExtractRequestFromArgs(args));
    }

    [Theory]
    [InlineData("test", "1,0386", "5,2722")]
    [InlineData("test", "0", "1,244")]
    [InlineData("test", "6,13893", "6,13894")]
    private void TestIfThrowExceptionWhenInvalidPriceRangeArgs(params string[] args)
    {
        Assert.Throws<InvalidPriceRangeException>(() => _controller.ExtractRequestFromArgs(args));
    }

    [Theory]
    [InlineData("test", "5,2722", "1.0386")]
    [InlineData("test", "1,244", "0,0923")]
    [InlineData("test", "6.13894", "6.13893")]
    private void TestIfCreateMonitoringRequestSuccessfully(params string[] args)
    {
        MonitoringRequest? request = null;
        var exception = Record.Exception(() => request = _controller.ExtractRequestFromArgs(args));
        Assert.Null(exception);
        Assert.NotNull(request);
        Assert.Equal(args[0], request.TicketName);
        Assert.Equal(double.Parse(args[1].Replace('.',',')), request.SaleRefPrice);
        Assert.Equal(double.Parse(args[2].Replace('.',',')), request.PurchaseRefPrice);
    }

    [Fact]
    private async Task TestIfSuccessfullyReturnResponseAndNotSendEmail()
    {
        var mockedRequest = _fixture.Create<MonitoringRequest>();

        var mockedStockQuote = _fixture.Create<StockQuoteDTO>();
        mockedStockQuote.AlertEmailSuggestion = AlertEmailSuggestionEnum.NONE;
        _monitoringServiceMock.Setup(x => x.CheckStockQuote(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()))
            .ReturnsAsync(mockedStockQuote);

        await _controller.Execute(mockedRequest);
        _monitoringServiceMock.Verify(
            x => x.CheckStockQuote(mockedRequest.TicketName, mockedRequest.SaleRefPrice,
                mockedRequest.PurchaseRefPrice), Times.Once());
        _emailServiceMock.Verify(
            x => x.SendAlertEmail(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<AlertEmailSuggestionEnum>()),
            Times.Never);
    }

    [Theory]
    [InlineData(AlertEmailSuggestionEnum.SALE)]
    [InlineData(AlertEmailSuggestionEnum.PURCHASE)]
    private async Task TestIfSuccessfullyReturnResponseAndSendEmail(AlertEmailSuggestionEnum alertEmailSuggestion)
    {
        var mockedRequest = _fixture.Create<MonitoringRequest>();

        var mockedStockQuote = _fixture.Create<StockQuoteDTO>();
        mockedStockQuote.AlertEmailSuggestion = alertEmailSuggestion;
        _monitoringServiceMock.Setup(x => x.CheckStockQuote(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()))
            .ReturnsAsync(mockedStockQuote);

        await _controller.Execute(mockedRequest);
        _monitoringServiceMock.Verify(
            x => x.CheckStockQuote(mockedRequest.TicketName, mockedRequest.SaleRefPrice,
                mockedRequest.PurchaseRefPrice), Times.Once());
        _emailServiceMock.Verify(
            x => x.SendAlertEmail(mockedRequest.TicketName, mockedStockQuote.Price!.Value,
                mockedStockQuote.AlertEmailSuggestion), Times.Once());
    }
}