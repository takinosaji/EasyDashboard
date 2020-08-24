module EasyDashboard.Api.Endpoints.Health
        
    open Giraffe
    open System.Runtime.Serialization
    open System.Reflection
    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks.V2.ContextInsensitive

    let private ApiVersion =
        let assembly = Assembly.GetEntryAssembly()
        let version = assembly.GetName().Version
        sprintf "%i.%i.%i" version.Major version.Minor version.Build
        
    let private UiVersion =
        sprintf "%i.%i.%i" 1 0 0
    
    [<DataContract>]
    type private HealthDto = {
        [<field: DataMember(Name="apiVersion")>]
        ApiVersion: string
        [<field: DataMember(Name="uiVersion")>]
        UiVersion: string
    }

    let handler: HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
              return! json { ApiVersion = ApiVersion
                             UiVersion = UiVersion } next ctx
            }