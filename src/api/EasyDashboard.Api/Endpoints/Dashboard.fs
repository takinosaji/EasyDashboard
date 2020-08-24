module EasyDashboard.Api.Endpoints.Dashboard

    open Giraffe
    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks.V2.ContextInsensitive
    
    let handler: HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
              do! Async.Sleep 10
              return! json "Dashboard hit" next ctx
            }