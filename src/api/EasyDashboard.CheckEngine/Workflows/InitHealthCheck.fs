module EasyDashboard.CheckEngine.InitHealthCheckWorkflow
        
    open EasyDashboard.Domain.AliasedTypes
    open EasyDashboard.Domain.Template.Dtos
    open EasyDashboard.Domain.Template.Models
    open EasyDashboard.Domain.Template.Factory
    open EasyDashboard.Domain.Health.Models

    open System
    open System.IO
    open System.Text.Json;
                  
    type InitHeathCheckCommand = {
        FolderPath: string
    }
    type CheckEngineState =
        | Working of EnvironmentHeartBeat IObservable
        | Idling
        | Faulted of string                 
    type InitHealthCheckUnitsAsync = InitHeathCheckCommand -> Async<CheckEngineState>    
    


    
    type WrapInObservable = EnvironmentTemplate -> EnvironmentHeartBeat IObservable


    type GetTemplatesPaths = string -> string array option
    let getTemplatePaths: GetTemplatesPaths =
        fun folderPath ->
            match Directory.GetFiles "*.json" with
            | [||] -> None
            | filePaths -> Some filePaths
    
    type GetTemplateContentAsync = string -> AsyncResult<string, string>
    let getTemplateContentAsync: GetTemplateContentAsync =
        fun templatePath ->
            async {
                try
                    let! content = File.ReadAllTextAsync templatePath |> Async.AwaitTask
                    return Ok content
                with
                | exn ->
                    let message = exn.ToString()
                    return Error message
            }
            
    type IncorrectTemplate = {
        Name: string
        Error: EnvironmentTemplateCreationError
    }
    type UnparsedTemplate = {
        Name: string
        Error: string
    }
    type ParseTemplateResult =
        | Correct of EnvironmentTemplate
        | WithErrors of IncorrectTemplate
        | Unrecognized of UnparsedTemplate
    type ParseTemplateCommand = {
        TemplateName: string
        TemplateContent: string
    }
    type ParseTemplate = ParseTemplateCommand -> ParseTemplateResult
    let parseTemplate: ParseTemplate =
        fun parseCommand ->
            try
                let templateDto = JsonSerializer.Deserialize<EnvironmentTemplateDto>(parseCommand.TemplateContent)
            with
            | exn -> Unrecognized {
                Name = parseCommand.TemplateName
                Error = exn.ToString()
            }
            match toEnvironmentModel templateDto with
            | Ok model -> Correct model
            | Error err ->
                WithErrors {
                    Name = parseCommand.TemplateName
                    Error = err
                }
                
            
    let initHealthCheckUnitsAsync: InitHealthCheckUnitsAsync =
        fun initCommand ->
            async {
                match getTemplatePaths initCommand.FolderPath with
                | None -> return Idling
                | Some paths ->
                    let parsingSequence = paths
                                        |> List.toSeq
                                        |> Seq.map (fun path -> getTemplateContentAsync path)
                                        
                    let! 
            }
//            
//            
//                
//    1) init engine
//        INIT STORE
//        directory ----> template file paths
//        readTemplateFromFile = string -> HealthCheckUnit
//        HealthcheckUnit  -> Observable with emiting updates for templates and with single static asnwer for unrecognized
//        save observables to engine         
//    
//    
//      type readTemplateFromFile = string -> EnvironmentTemplate
//            string - Json
//            json - Dtos
//            dto - model
//          add template to engine
//          
//     2) unitObservableFactory unit: HealthCheckUnit =
//         match unit with
//            | Environment template -> toParsedEnvironment EnvironmentTemplate
//            | UnrecognizedTemplate errorInfo -> toUnrecognizedTemplateObservable errorInfo
//        
//         parsedEnv observable
//               url -> parsing function Factory -> summary Dto
//                parsing function from factory takes string and parses to Dto
//            dto - toModel
//     
//     3) Processing iteration (Subscription)
//       
//            send model to websockets (websocket functon is secondary adapter, typ is port)
//         
//          
//          
//    3) get environment state from engine workflow
//        get summary
//        summary ---> summary dto
//    
//    
//    
//    4) Update environment health
//        template -----> content
//        content ------> summary
//        
//  
//        
//        
//        
//        COmmands as input
//    WHAT IF there are issues with FS - for instance filepath is broken