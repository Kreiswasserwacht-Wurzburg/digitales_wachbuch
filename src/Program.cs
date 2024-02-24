using GraphQL;
using GraphQL.Types;
using GraphQL.MicrosoftDI;
using DigitalGuardBook.Data;
using DigitalGuardBook.Repositories;
using DigitalGuardBook.GraphQL;
using System.Net.WebSockets;


namespace DigitalGuardBook;

public class Program
{
    private static List<WebSocket> wsClients = new List<WebSocket>();
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
        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {

                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                wsClients.Add(webSocket);
                await WsKeepAlive(context, webSocket);
                

                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await next();
            }
        });

        app.Run();
    }

    private static async Task WsKeepAlive(HttpContext context, WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        while (!result.CloseStatus.HasValue)
        {
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }
        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }

    public static void Notify()
    {

        foreach (WebSocket client in wsClients)
        {
            client.SendAsync(new ArraySegment<byte>(new byte[1]),WebSocketMessageType.Binary,true,CancellationToken.None);
        }
    }
}

