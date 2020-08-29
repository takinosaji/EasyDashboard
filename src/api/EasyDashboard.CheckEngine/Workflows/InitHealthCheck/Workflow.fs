module EasyDashboard.CheckEngine.InitHealthCheck.Workflow
        
    open EasyDashboard.CheckEngine.Workflows.InitHealthCheck.HealthObservableProvider
    open EasyDashboard.Domain.Environment.Template.Factory
    open EasyDashboard.Domain.Environment.Template.Factory.DTOs
    open EasyDashboard.Domain.Environment.Template.Provider

    open System.Text.Json

    
    type ParseTemplate = AcquiredTemplateContent -> ParsedTemplate
    let parseTemplate: ParseTemplate =
        fun template ->
            try
                let templateDto = JsonSerializer.Deserialize<EnvironmentTemplateDto>(template.Content)
                match toEnvironmentModel templateDto with
                | Ok model -> Correct model
                | Error err ->
                    WithErrors {
                        Name = template.Filename
                        Error = err
                    }
            with
            | exn -> Unrecognized {
                Name = template.Filename
                Error = exn.ToString()
            }
             
    type ProcessTemplateAsync = RequestedTemplate Async -> ProcessedTemplate Async  
    let processTemplateAsync: ProcessTemplateAsync =
        fun requestedTemplateAsync ->
            async {
                let! requestedTemplate = requestedTemplateAsync
                match requestedTemplate with
                | TemplateError failedRequest ->
                    return Faulted {
                        Name = failedRequest.Filename
                        Error = failedRequest.Error
                    }
                | TemplateContent successfulRequest ->
                    return Processed (parseTemplate successfulRequest)    
             }
            
    type InitHeathCheckCommand = {
        FolderPath: string
    }
    type CheckEngine =
        | Working of EnvironmentHeartBeatCreationResult seq
        | Idling
        | Faulted of string                 
    type InitHealthCheckEngineAsync =
        HealthObservableAsyncProvider -> TemplateAsyncProvider -> InitHeathCheckCommand -> CheckEngine Async 
    let initHealthCheckEngineAsync: InitHealthCheckEngineAsync =
        fun toHealthObservableAsync templateAsyncProvider initCommand  ->
            async {
               let availableTemplates = templateAsyncProvider { FolderPath = initCommand.FolderPath }
               match availableTemplates with
               | Error err -> return Faulted err
               | Ok None -> return Idling
               | Ok (Some requestedTemplates) ->
                    let! environmentHearts =
                        requestedTemplates
                        |> Seq.map processTemplateAsync
                        |> Seq.map toHealthObservableAsync
                        |> Async.Sequential
                    return Working (environmentHearts)      
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