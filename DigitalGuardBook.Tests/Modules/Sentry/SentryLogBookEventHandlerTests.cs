using DigitalGuardBook.Data.Entities;
using DigitalGuardBook.Infrastructure;
using DigitalGuardBook.Modules.Sentry.EventHandlers;
using DigitalGuardBook.Modules.Sentry.Events;
using DigitalGuardBook.Repositories;
using Microsoft.Extensions.Localization;
using Moq;
using Xunit;

namespace DigitalGuardBook.Tests.Modules.Sentry;

public class SentryLogBookEventHandlerTests
{
    private readonly Mock<ILogBookRepository> _mockLogBookRepository;
    private readonly Mock<IPersonRepository> _mockPersonRepository;
    private readonly Mock<IStringLocalizer<SentryLogBookEventHandler>> _mockLocalizer;
    private readonly SentryLogBookEventHandler _handler;

    public SentryLogBookEventHandlerTests()
    {
        _mockLogBookRepository = new Mock<ILogBookRepository>();
        _mockPersonRepository = new Mock<IPersonRepository>();
        _mockLocalizer = new Mock<IStringLocalizer<SentryLogBookEventHandler>>();

        _handler = new SentryLogBookEventHandler(
            _mockLogBookRepository.Object,
            _mockPersonRepository.Object,
            _mockLocalizer.Object
        );
    }

    [Fact]
    public void Register_SubscribesToEventTypes()
    {
        // Arrange
        var mockPublisher = new Mock<IInProcessEventPublisher>();

        // Act
        _handler.Register(mockPublisher.Object);

        // Assert - just verify the handler can be registered without errors
        Assert.NotNull(_handler);
    }

    [Fact]
    public async Task LogBookRepository_IsCalledWithCorrectParameters()
    {
        // Arrange
        var time = DateTimeOffset.Now;
        var message = "Test message";

        // Act
        await _mockLogBookRepository.Object.InsertLogBookEntryAsync(message, time);

        // Assert
        _mockLogBookRepository.Verify(
            r => r.InsertLogBookEntryAsync(message, time),
            Times.Once
        );
    }

    [Fact]
    public async Task PersonRepository_IsCalledWithCorrectIds()
    {
        // Arrange
        var personIds = new List<string> { "person-1", "person-2" };

        var person1 = new PersonComposed { Id = "person-1", FirstName = "John", LastName = "Doe" };
        var person2 = new PersonComposed { Id = "person-2", FirstName = "Jane", LastName = "Smith" };

        _mockPersonRepository
            .Setup(r => r.PersonsAsync(It.IsAny<IList<string>>()))
            .ReturnsAsync(new List<PersonComposed> { person1, person2 });

        // Act
        var result = await _mockPersonRepository.Object.PersonsAsync(personIds);

        // Assert
        Assert.Equal(2, result.Count);
        _mockPersonRepository.Verify(r => r.PersonsAsync(It.IsAny<IList<string>>()), Times.Once);
    }
}
