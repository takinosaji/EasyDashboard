module EasyDashboard.CheckEngine.InitHealthCheck.FileSystem

    open EasyDashboard.Domain.Template.Provider

    open System.IO
        
    let getTemplatePaths folderPath =
        try
            match Directory.GetFiles "*.json" with
            | [||] -> Ok None
            | filePaths -> Ok (Some filePaths)

         with
            | exn -> Error(exn.ToString())
    
    let getRequestTemplateCommands (paths: string array) :RequestTemplateCommand seq =
        paths
        |> Array.toSeq
        |> Seq.map (fun path -> {
              Filename = Path.GetFileName path
              Read = File.ReadAllTextAsync path |> Async.AwaitTask
            })
        
    let getTemplateSequence (commandSequence: RequestTemplateCommand seq) =
        Seq.map (fun command ->
            async {
                try
                    let! content = command.Read
                    return TemplateContent {
                        Filename = command.Filename
                        Content = content
                    }
                with
                | exn ->
                    return TemplateError {
                        Filename = command.Filename
                        Error = exn.ToString()
                    }
            }) commandSequence
    
    // TODO: Consider implementation of nonempty sequence 
    let getTemplatesFromFolderSequence: GetTemplateSequence =
        fun command ->
                try
                    match getTemplatePaths command.FolderPath with
                    | Error err -> Error err
                    | Ok lookupResult ->
                        match lookupResult with
                        | None -> Ok None
                        | Some paths ->
                            let templates = paths
                                            |> getRequestTemplateCommands
                                            |> getTemplateSequence          
                            Ok (Some templates)
                with
                | exn -> Error(exn.ToString())