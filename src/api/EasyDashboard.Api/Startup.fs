namespace EasyDashboard.Api

open System.IO
open Suave
open System.Threading
open Microsoft.Extensions.Configuration

open EasyDashboard.Api.Endpoints

module Startup =
    let [<Literal>] private ConfigSectionName = "Hosting"        
    let [<Literal>] private PortField = "Port"
    let [<Literal>] private HostField = "Host"
    let [<Literal>] private HomeFolderField = "HomeFolder"

    type ApiStartupSettings = {
        Host: string
        Port: int
        HomeFolder: string
    }
        
    let ExtractStartupSettings (configuration: IConfiguration) =
        let hostingSection = configuration.GetSection ConfigSectionName
        {
            Host = hostingSection.GetValue(HostField)
            Port = hostingSection.GetValue(PortField)
            HomeFolder = hostingSection.GetValue(HomeFolderField)
        }

    let Run (webAppConfig: ApiStartupSettings) =
        let cts = new CancellationTokenSource()
        let suaveConfig = {
            defaultConfig with
            cancellationToken = cts.Token
            bindings = [ HttpBinding.createSimple HTTP webAppConfig.Host webAppConfig.Port ]
            homeFolder = Some(Path.GetFullPath webAppConfig.HomeFolder)
        }

        let app =
          choose [
            Static.EntryPoint webAppConfig.HomeFolder
            Static.AssetFiles
            Static.NotFoundHandler
          ]

        let server = startWebServerAsync suaveConfig app |> snd
          
        Async.Start(server, cts.Token)       
        cts