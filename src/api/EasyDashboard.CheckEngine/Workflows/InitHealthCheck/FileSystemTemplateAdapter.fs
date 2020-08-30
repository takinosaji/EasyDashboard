module EasyDashboard.CheckEngine.InitHealthCheck.FileSystemTemplateProvider

    open EasyDashboard.Domain.Environment.Template.Ports

    open System.IO
        
    let getTemplatePaths folderPath =
        try
            let templatePaths = Directory.GetFiles(folderPath, "*.json")
            match templatePaths with
            | [||] -> Ok None
            | filePaths -> Ok (Some filePaths)

         with
            | exn -> Error(exn.ToString())
    type RequestTemplateCommand = {
        Filename: string
        Read: string Async
    }
    let getRequestTemplateActions (path: string) :RequestTemplateAction =
        fun () ->
            async {
                let fileName = Path.GetFileName path
                try
                    let! content = File.ReadAllTextAsync path |> Async.AwaitTask
                    return TemplateContent {
                        Filename = fileName
                        Content = content
                    }
                with
                | exn ->
                    return TemplateError {
                        Filename = fileName
                        Error = exn.ToString()
                    }
            }
  
    // TODO: Consider implementation of nonempty sequence 
    let getTemplatesFromFSAsync: TemplateAsyncProvider =
        fun command ->
                try
                    match getTemplatePaths command.FolderPath with
                    | Error err -> Error err
                    | Ok lookupResult ->
                        match lookupResult with
                        | None -> Ok None
                        | Some paths ->    
                            Ok (Some (paths
                                |> Array.toSeq
                                |> Seq.map getRequestTemplateActions))
                with
                | exn -> Error(exn.ToString())