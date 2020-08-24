module EasyDashboard.Api.Endpoints.Health
    
    open EasyDashboard.Domain.Versioning
    
    open Giraffe
    open System.Runtime.Serialization
    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks.V2.ContextInsensitive

    [<DataContract>]
    type HealthDto = {
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