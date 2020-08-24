module EasyDashboard.Api.Configuration

    open Microsoft.AspNetCore.Hosting
    open Microsoft.Extensions.Configuration
    open Microsoft.Extensions.Logging
    open Microsoft.Extensions.DependencyInjection
    open Giraffe
    open Microsoft.AspNetCore.Builder

    open EasyDashboard.Api.Endpoints
    open EasyDashboard.Api.Settings
        
    let [<Literal>] dashboardRoute = "/dashboard"
    let [<Literal>] healthRoute = "/health"
    let api =
        choose [
            GET >=> choose
                [ route dashboardRoute >=> Dashboard.handler
                  route healthRoute >=> Health.handler]
        ]
        
    type Startup(config: IConfiguration) =        
        member _.ConfigureServices (services : IServiceCollection) =            
            services
                .AddGiraffe() |> ignore
                
        member _.Configure (app : IApplicationBuilder) (env: IWebHostEnvironment) =
            let startupSettings = extractStartupSettings config
            app.UseGiraffeErrorHandler(Error.handler)
               .UseStaticFiles(startupSettings.UiDirectory)
               .UseDefaultFiles()
               .UseGiraffe api             
        
    let configureLogging (loggerBuilder : ILoggingBuilder) =
        loggerBuilder.AddFilter(fun lvl -> lvl.Equals LogLevel.Error)
                     .AddConsole()
                     .AddDebug() |> ignore