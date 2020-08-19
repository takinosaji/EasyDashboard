module EasyDashboard.Api.Startup

    open System.IO
    open Suave
    open Suave.Filters
    open Suave.Operators
    open Suave.WebSocket
    open System.Threading
    open Microsoft.Extensions.Configuration

    open EasyDashboard.Api.Endpoints

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
            GET >=> choose
                [ path "/dashboard" >=> Dashboard.handler
                  path "/health" >=> Health.handler
                  path "/websocket" >=> handShake LiveUpdates.handler]
            Static.EntryPoint webAppConfig.HomeFolder
            Static.AssetFiles
            Static.NotFoundHandler
          ]

        let server = startWebServerAsync suaveConfig app |> snd
          
        Async.Start(server, cts.Token)       
        cts