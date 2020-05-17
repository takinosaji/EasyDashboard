namespace EasyDashboard.Api

open System.IO
open System.Threading
open Suave
open Microsoft.Extensions.Configuration

module Startup =
    
    module Constants =
        let [<Literal>] ConfigSectionName = "Hosting"        
        let [<Literal>] PortField = "Host"
        let [<Literal>] HostField = "Port"
        let [<Literal>] HomeFolderField = "Hosting"

    type ApiStartupSettings = {
        Host: string
        Port: int
        HomeFolder: string
    }
        
    let ExtractStartupSettings (configuration: IConfiguration) =
        let hostingSection = configuration.GetSection Constants.ConfigSectionName
        let settings = {
            Host = hostingSection.GetValue(Constants.HostField)
            Port = hostingSection.GetValue(Constants.PortField)
            HomeFolder = hostingSection.GetValue(Constants.HomeFolderField)
        }
        settings

    let Run (webAppConfig: ApiStartupSettings) =
        let cts = new CancellationTokenSource()
        let conf = {
            defaultConfig with
            cancellationToken = cts.Token
            bindings = [ HttpBinding.createSimple HTTP webAppConfig.Host webAppConfig.Port ]
            homeFolder = Some(Path.GetFullPath webAppConfig.HomeFolder)
        }

        let app =
          choose [
              
          ]

        let server = startWebServerAsync conf (Successful.OK "Hello World") |> snd
          
        Async.Start(server, cts.Token)       
        cts