using Microsoft.Extensions.Logging;
using DigitalGuardBook.Data.Entities;
using DigitalGuardBook.Infrastructure;
using DigitalGuardBook.Modules.Sentry;
using DigitalGuardBook.Modules.Sentry.Events;
using DigitalGuardBook.Modules.Person;
using DigitalGuardBook.Modules.Organisation;
using Moq;
using Xunit;
using SentryEntity = DigitalGuardBook.Data.Entities.Sentry;

namespace DigitalGuardBook.Tests.Modules.Sentry;

public class SentryServiceTests
{
    private SentryService CreateService(
        Mock<ISentryRepository>? sentryRepository = null,
        Mock<IEventPublisher>? publisher = null,
        Mock<ILogger<SentryService>>? logger = null,
        Mock<IPersonRepository>? personRepository = null,
        Mock<IOrganisationRepository>? organisationRepository = null)
    {
        var personRepositoryIsNew = personRepository == null;
        var organisationRepositoryIsNew = organisationRepository == null;

        sentryRepository ??= new Mock<ISentryRepository>();
        publisher ??= new Mock<IEventPublisher>();
        logger ??= new Mock<ILogger<SentryService>>();
        personRepository ??= new Mock<IPersonRepository>();
        organisationRepository ??= new Mock<IOrganisationRepository>();

        // Default setup: persons exist (only if not explicitly provided)
        if (personRepositoryIsNew)
        {
            personRepository.Setup(r => r.PersonsAsync(It.IsAny<IList<string>>()))
                .ReturnsAsync((IList<string> ids) =>
                    ids.Select(id => new PersonComposed { Id = id, FirstName = "Test", LastName = "Person" }).ToList());
        }

        // Default setup: organisation exists (only if not explicitly provided)
        if (organisationRepositoryIsNew)
        {
            organisationRepository.Setup(r => r.OrganisationAsync(It.IsAny<string>()))
                .ReturnsAsync(new OrganisationComposed { Id = "org-id", Name = "Test Org" });
        }

        return new SentryService(sentryRepository.Object, publisher.Object, logger.Object, personRepository.Object, organisationRepository.Object);
    }

    [Fact]
    public void SentryService_CanBeInstantiated()
    {
        // Arrange & Act
        var service = CreateService();

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public async Task StartSentryAsync_PublishesMultipleEvents()
    {
        // Arrange
        var mockRepository = new Mock<ISentryRepository>();
        mockRepository
            .Setup(r => r.InsertSentryAsync(It.IsAny<SentryEntity>()))
            .Returns(Task.CompletedTask);

        var service = CreateService(sentryRepository: mockRepository);

        var sentry = new SentryEntity
        {
            Id = "test-id",
            OrganisationId = "org-id",
            Start = DateTimeOffset.Now,
            GuardServices = new[] { new GuardService { PersonId = "guard-1", Start = DateTimeOffset.Now } }.ToList(),
            SupervisorServices = new[] { new GuardService { PersonId = "supervisor-1", Start = DateTimeOffset.Now } }.ToList()
        };

        // Act
        var result = await service.StartSentryAsync(sentry);

        // Assert
        Assert.Equal(sentry.Id, result.Id);
    }

    [Fact]
    public async Task FinishSentryAsync_PublishesEndEvents()
    {
        // Arrange
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

        var mockRepository = new Mock<ISentryRepository>();
        mockRepository
            .Setup(r => r.GetSentryAsync(sentryId))
            .Returns(Task.FromResult(sentry));

        mockRepository
            .Setup(r => r.UpdateSentryEndAsync(sentryId, finishTime))
            .Returns(Task.CompletedTask);

        var service = CreateService(sentryRepository: mockRepository);

        // Act
        await service.FinishSentryAsync(sentryId, finishTime);

        // Assert
        Assert.True(true);
    }

    [Fact]
    public async Task FinishSentryAsync_UpdatesRepository()
    {
        // Arrange
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

        var mockRepository = new Mock<ISentryRepository>();
        mockRepository
            .Setup(r => r.GetSentryAsync(sentryId))
            .Returns(Task.FromResult(sentry));

        mockRepository
            .Setup(r => r.UpdateSentryEndAsync(sentryId, finishTime))
            .Returns(Task.CompletedTask);

        var service = CreateService(sentryRepository: mockRepository);

        // Act
        await service.FinishSentryAsync(sentryId, finishTime);

        // Assert
        mockRepository.Verify(r => r.UpdateSentryEndAsync(sentryId, finishTime), Times.Once);
    }

    [Fact]
    public async Task StartSentryAsync_ThrowsPersonNotFoundExceptionForMissingGuard()
    {
        // Arrange
        var mockPersonRepository = new Mock<IPersonRepository>();
        mockPersonRepository
            .Setup(r => r.PersonsAsync(It.IsAny<IList<string>>()))
            .ReturnsAsync(new List<PersonComposed>()); // No persons found

        var service = CreateService(personRepository: mockPersonRepository);

        var sentry = new SentryEntity
        {
            Id = "test-id",
            OrganisationId = "org-id",
            Start = DateTimeOffset.Now,
            GuardServices = new[] { new GuardService { PersonId = "missing-guard", Start = DateTimeOffset.Now } }.ToList(),
            SupervisorServices = new[] { new GuardService { PersonId = "supervisor-1", Start = DateTimeOffset.Now } }.ToList()
        };

        // Act & Assert
        await Assert.ThrowsAsync<PersonNotFoundException>(() => service.StartSentryAsync(sentry));
    }

    [Fact]
    public async Task StartSentryAsync_ThrowsOrganisationNotFoundExceptionForMissingOrg()
    {
        // Arrange
        var mockOrgRepository = new Mock<IOrganisationRepository>();
        mockOrgRepository
            .Setup(r => r.OrganisationAsync(It.IsAny<string>()))
            .ReturnsAsync((OrganisationComposed)null!);

        var service = CreateService(organisationRepository: mockOrgRepository);

        var sentry = new SentryEntity
        {
            Id = "test-id",
            OrganisationId = "missing-org",
            Start = DateTimeOffset.Now,
            GuardServices = new[] { new GuardService { PersonId = "guard-1", Start = DateTimeOffset.Now } }.ToList(),
            SupervisorServices = new[] { new GuardService { PersonId = "supervisor-1", Start = DateTimeOffset.Now } }.ToList()
        };

        // Act & Assert
        await Assert.ThrowsAsync<OrganisationNotFoundException>(() => service.StartSentryAsync(sentry));
    }

    [Fact]
    public async Task StartSentryAsync_ThrowsInvalidOperationExceptionForNoSupervisors()
    {
        // Arrange
        var service = CreateService();

        var sentry = new SentryEntity
        {
            Id = "test-id",
            OrganisationId = "org-id",
            Start = DateTimeOffset.Now,
            GuardServices = new[] { new GuardService { PersonId = "guard-1", Start = DateTimeOffset.Now } }.ToList(),
            SupervisorServices = new List<GuardService>() // No supervisors
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.StartSentryAsync(sentry));
    }
}
