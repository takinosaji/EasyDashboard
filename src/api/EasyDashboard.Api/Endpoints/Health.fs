module EasyDashboard.Api.Endpoints.Health
    
    open EasyDashboard.Api.Utils
    open EasyDashboard.Domain.Versioning
    
    open System.Runtime.Serialization
    open Suave
    open Suave.Successful 

    [<DataContract>]
    type HealthDto = {
        [<field: DataMember(Name="apiVersion")>]
        ApiVersion: string
        [<field: DataMember(Name="uiVersion")>]
        UiVersion: string
    }
    
    let handler: WebPart =
        fun (x : HttpContext) ->
            async {
                return! OK ({ ApiVersion = ApiVersion
                              UiVersion = UiVersion } |> toJson) x
            }