module EasyDashboard.Api.Endpoints.Static

open Suave
open Suave.Filters
open Suave.Operators

    let [<Literal>] private IndexFileName = "index.html"
    
    let EntryPoint homeFolder = GET >=> path "/" >=> Files.browseFileHome IndexFileName        
    let AssetFiles = GET >=> Files.browseHome        
    let NotFoundHandler = RequestErrors.NOT_FOUND "Page not found."