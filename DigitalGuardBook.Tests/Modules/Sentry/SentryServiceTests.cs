using DigitalGuardBook.Data.Entities;
using DigitalGuardBook.Infrastructure;
using DigitalGuardBook.Modules.Sentry;
using DigitalGuardBook.Modules.Sentry.Events;
using Moq;
using Xunit;
using SentryEntity = DigitalGuardBook.Data.Entities.Sentry;

namespace DigitalGuardBook.Tests.Modules.Sentry;

public class SentryServiceTests
{
    [Fact]
    public void SentryService_CanBeInstantiated()
    {
        // Arrange & Act
        var mockRepository = new Mock<ISentryRepository>();
        var mockPublisher = new Mock<IEventPublisher>();
        var service = new SentryService(mockRepository.Object, mockPublisher.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public async Task StartSentryAsync_PublishesMultipleEvents()
    {
        // Arrange
        var mockRepository = new Mock<ISentryRepository>();
        var mockPublisher = new Mock<IEventPublisher>();
        var service = new SentryService(mockRepository.Object, mockPublisher.Object);

        var sentry = new SentryEntity
        {
            Id = "test-id",
            OrganisationId = "org-id",
            Start = DateTimeOffset.Now,
            GuardServices = new[] { new GuardService { PersonId = "guard-1", Start = DateTimeOffset.Now } }.ToList(),
            SupervisorServices = new[] { new GuardService { PersonId = "supervisor-1", Start = DateTimeOffset.Now } }.ToList()
        };

        mockRepository
            .Setup(r => r.InsertSentryAsync(It.IsAny<SentryEntity>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await service.StartSentryAsync(sentry);

        // Assert
        Assert.Equal(sentry.Id, result.Id);
        mockPublisher.Verify(p => p.PublishAsync(It.IsAny<object>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task FinishSentryAsync_PublishesEndEvents()
    {
        // Arrange
        var mockRepository = new Mock<ISentryRepository>();
        var mockPublisher = new Mock<IEventPublisher>();
        var service = new SentryService(mockRepository.Object, mockPublisher.Object);

        var sentryId = "test-id";
        var finishTime = DateTimeOffset.Now;
        var sentry = new SentryEntity
        {
            Id = sentryId,
            OrganisationId = "org-id",
            Start = finishTime.AddHours(-2),
            GuardServices = new[] { new GuardService { PersonId = "guard-1", Start = finishTime.AddHours(-2) } }.ToList(),
            SupervisorServices = new[] { new GuardService { PersonId = "supervisor-1", Start = finishTime.AddHours(-2) } }.ToList()
        };

        mockRepository
            .Setup(r => r.GetSentryAsync(sentryId))
            .Returns(Task.FromResult(sentry));

        mockRepository
            .Setup(r => r.UpdateSentryEndAsync(sentryId, finishTime))
            .Returns(Task.CompletedTask);

        // Act
        await service.FinishSentryAsync(sentryId, finishTime);

        // Assert
        mockPublisher.Verify(p => p.PublishAsync(It.IsAny<object>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task FinishSentryAsync_UpdatesRepository()
    {
        // Arrange
        var mockRepository = new Mock<ISentryRepository>();
        var mockPublisher = new Mock<IEventPublisher>();
        var service = new SentryService(mockRepository.Object, mockPublisher.Object);

        var sentryId = "test-id";
        var finishTime = DateTimeOffset.Now;
        var sentry = new SentryEntity
        {
            Id = sentryId,
            OrganisationId = "org-id",
            Start = finishTime.AddHours(-1),
            GuardServices = new List<GuardService>(),
            SupervisorServices = new List<GuardService>()
        };

        mockRepository
            .Setup(r => r.GetSentryAsync(sentryId))
            .Returns(Task.FromResult(sentry));

        mockRepository
            .Setup(r => r.UpdateSentryEndAsync(sentryId, finishTime))
            .Returns(Task.CompletedTask);

        // Act
        await service.FinishSentryAsync(sentryId, finishTime);

        // Assert
        mockRepository.Verify(r => r.UpdateSentryEndAsync(sentryId, finishTime), Times.Once);
    }
}
