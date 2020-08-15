module EasyDashboard.Host.WorkerService

    open Microsoft.Extensions.Configuration
    open Microsoft.Extensions.Hosting
    open Microsoft.Extensions.Logging

    type WorkerService(logger : ILogger<WorkerService>,
                       configuration: IConfiguration) =
        inherit BackgroundService()
        
        let _logger = logger
        
        override bs.ExecuteAsync _ =
            let asyncExpression = async {
                EasyDashboard.Api.Startup.ExtractStartupSettings configuration |>
                EasyDashboard.Api.Startup.Run |>
                ignore
            }
            
            upcast Async.StartAsTask asyncExpression