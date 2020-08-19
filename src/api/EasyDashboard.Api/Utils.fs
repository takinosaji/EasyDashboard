module EasyDashboard.Api.Utils

    open System.Text
    open Suave

    let toJson obj =
        Json.toJson obj |> Encoding.UTF8.GetString

