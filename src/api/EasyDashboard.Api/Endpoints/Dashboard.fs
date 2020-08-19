module EasyDashboard.Api.Endpoints.Dashboard

    open Suave
    open Suave.Successful 

    let handler: WebPart =
        fun (x : HttpContext) ->
            async {
              do! Async.Sleep 10
              return! OK "Dashboard hit" x
            }