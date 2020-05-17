namespace EasyDashboard

open Microsoft.Extensions.Configuration
open System.Threading.Tasks
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

type WorkerService(logger : ILogger<WorkerService>,
                   configuration: IConfiguration) =
    inherit BackgroundService()
    
    let _logger = logger
    
    override bs.ExecuteAsync stoppingToken =
        let f : Async<unit> = async {
            let hostingSection = configuration.GetSection "Hosting"
            EasyDashboard.Api.Init (hostingSection.GetValue("Host")) (hostingSection.GetValue("Port")) |> ignore
        }
        
        Async.StartAsTask f :> Task