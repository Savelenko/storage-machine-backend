open System
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Builder
open Giraffe
open Thoth.Json.Giraffe
open Thoth.Json.Net
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Authentication.Cookies
open Microsoft.Extensions.Configuration
open Microsoft.AspNetCore.Hosting
open StorageMachine
open StorageMachine.Stock
open StorageMachine.Repacking

/// Assembles the ASP.NET "application" from various framework modules.
let private configureApp (app: IApplicationBuilder) =
    let errorHandler (ex: Exception) (log: ILogger) =
        log.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")
        clearResponse >=> setStatusCode 500

    app
        .UseHsts()
        .UseGiraffeErrorHandler(errorHandler)
        .UseStaticFiles()
        .UseAuthentication()
        .UseAuthorization()
        // Provide Giraffe with combined HTTP handlers
        .UseGiraffe(HttpHandlers.requestHandlers)

/// Prepares dependency injection consisting of framework and application-specific components.
let private configureServices (services: IServiceCollection) =
    services
        .AddHsts(fun options -> options.MaxAge <- TimeSpan.FromDays(180.0))
        .AddAuthorization()
        .AddAuthentication(fun options -> options.DefaultScheme <- CookieAuthenticationDefaults.AuthenticationScheme)
        .Services
        // Data access implementation of the Stock component
        .AddSingleton<Stock.IStockDataAccess>(Stock.stockPersistence)
        // Data access implementation of the Repacking component
        .AddSingleton<Repacking.IBinTreeDataAccess>(Repacking.binTreeDataAccess)
        .AddGiraffe()
        .AddSingleton<Json.ISerializer>(ThothSerializer (skipNullField = false, caseStrategy = CaseStrategy.CamelCase))
        |> ignore

/// The main entry point of the back-end.
[<EntryPoint>]
let main argv =
    try
        Host
            .CreateDefaultBuilder(argv)
            .ConfigureWebHostDefaults(fun webHostBuilder ->
                webHostBuilder
                    .ConfigureServices(configureServices)
                    .Configure(configureApp)
                |> ignore
            )
            .UseConsoleLifetime(fun opts -> opts.SuppressStatusMessages <- true)
            .Build()
            .Run()
        0
    with
    | ex -> 1