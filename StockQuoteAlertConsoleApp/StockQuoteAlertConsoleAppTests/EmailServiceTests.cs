using System.Net.Mail;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using StockQuoteAlertConsoleApp.Enums;
using StockQuoteAlertConsoleApp.exceptions;
using StockQuoteAlertConsoleApp.models.Adapters;
using StockQuoteAlertConsoleApp.models.Services;
using StockQuoteAlertConsoleApp.Services;

namespace StockQuoteAlertConsoleAppTests;

public class EmailServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<ISmtpClientAdapter> _smtpClientAdapterMock;

    public EmailServiceTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _smtpClientAdapterMock = _fixture.Freeze<Mock<ISmtpClientAdapter>>();
    }

    private IEmailService GetEmailService(string alertEmailSender = "test",
        string alertEmailDestination = "test") =>
        new EmailService(_smtpClientAdapterMock.Object, alertEmailSender, alertEmailDestination);

    [Fact]
    private async Task TestIfThrowInternalExceptionWhenIncorrectMail()
    {
        var mockedTicket = _fixture.Create<string>();
        var mockedPrice = _fixture.Create<double>();
        var mockedAlertEmailSuggestion = _fixture.Create<AlertEmailSuggestionEnum>();
        var emailService = GetEmailService();

        await Assert.ThrowsAsync<InternalException>(async () =>
            await emailService.SendAlertEmail(mockedTicket, mockedPrice, mockedAlertEmailSuggestion));
        _smtpClientAdapterMock.Verify(x => x.SendMailAsync(It.IsAny<MailMessage>()), Times.Never);
    }

    [Theory]
    [InlineData("test@test.com.br", "test@test.com")]
    [InlineData("test2@test.com.br", "test2@test.com")]
    [InlineData("test_3@test.com.br", "test_3@test.com")]
    private async Task TestIfThrowInternalExceptionWhenSendMailThrows(string alertEmailSender,
        string alertEmailDestination)
    {
        var mockedTicket = _fixture.Create<string>();
        var mockedPrice = _fixture.Create<double>();
        var mockedAlertEmailSuggestion = _fixture.Create<AlertEmailSuggestionEnum>();
        var emailService = GetEmailService(alertEmailSender, alertEmailDestination);

        _smtpClientAdapterMock.Setup(x => x.SendMailAsync(It.IsAny<MailMessage>())).ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<InternalException>(async () =>
            await emailService.SendAlertEmail(mockedTicket, mockedPrice, mockedAlertEmailSuggestion));
        _smtpClientAdapterMock.Verify(x => x.SendMailAsync(It.IsAny<MailMessage>()), Times.Once);
    }
}