using GraphQL;
using GraphQL.Types;
using GraphQL.MicrosoftDI;
using DigitalGuardBook.Data;
using DigitalGuardBook.Repositories;
using DigitalGuardBook.GraphQL;

namespace DigitalGuardBook;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
        builder.Services.AddRequestLocalization(options =>
        {
            var supportedCultures = new[] { "en", "de-DE" };
            options.SetDefaultCulture(supportedCultures.First())
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);
        });

        builder.Services.AddSingleton(x => new DigitalGuardBookDataContext(builder.Configuration.GetConnectionString("MongoConnection")));
        builder.Services.AddSingleton<PersonRepository>();
        builder.Services.AddSingleton<OrganisationRepository>();
        builder.Services.AddSingleton<StationRepository>();
        builder.Services.AddSingleton<LogBookRepository>();
        builder.Services.AddSingleton<SentryRepository>();


        // Add GraphQL
        builder.Services.AddSingleton<ISchema, DigitalGuardBookScheme>(services => new DigitalGuardBookScheme(new SelfActivatingServiceProvider(services)));
        builder.Services.AddGraphQL(builder => builder
            .AddSystemTextJson()
            .AddSchema<DigitalGuardBookScheme>());

        var app = builder.Build();

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            ApplyCurrentCultureToResponseHeaders = true
        });

        app.UseDeveloperExceptionPage();
        app.UseWebSockets();
        app.UseGraphQL("/graphql");            // url to host GraphQL endpoint
        app.UseGraphQLGraphiQL("/");

        app.Run();
    }
}
