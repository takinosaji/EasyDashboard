module EasyDashboard.CheckEngine.InitHealthCheck.Workflow
        
    open EasyDashboard.CheckEngine.Workflows.InitHealthCheck.HealthObservableProvider
    open EasyDashboard.Domain.Environment.Template.Parsing
    open EasyDashboard.Domain.Environment.Template.Ports
                 
    type ProcessTemplateAsync = (unit -> RequestedTemplate Async) -> ProcessedTemplate Async  
    let processTemplateAsync: ProcessTemplateAsync =
        fun requestTemplateAsync ->
            async {
                let! requestedTemplate = requestTemplateAsync()
                match requestedTemplate with
                | TemplateError failedRequest ->
                    return Faulted {
                        Name = failedRequest.Filename
                        Error = failedRequest.Error
                    }
                | TemplateContent successfulRequest ->
                    return Parsed (parseTemplate successfulRequest)    
             }
         
    // TODO: Substitute active pattern with regular function when this logic will have to become injectable   
    let (|HaveConflicts|_|) (templates: ProcessedTemplate seq) =
        templates
        |> Seq
        
    type InitHeathCheckCommand = {
        FolderPath: string
    }
    type CheckEngineState =
        | Initializing
        | Working of EnvironmentHeartBeatCreationResult seq
        | Idling
        | Faulted of string                 
    type InitHealthCheckEngineAsync =
         TemplateAsyncProvider -> HealthObservableAsyncProvider -> InitHeathCheckCommand -> CheckEngineState Async 
    let initHealthCheckEngineAsync: InitHealthCheckEngineAsync =
        fun toHealthObservableAsync provideTemplatesAsync initCommand  ->
            async {
               let availableTemplates = provideTemplatesAsync { FolderPath = initCommand.FolderPath }
               match availableTemplates with
               | Error err -> return Faulted err
               | Ok None -> return Idling
               | Ok (Some requestedTemplates) ->
                    let! templates =
                        requestedTemplates 
                        |> Seq.map processTemplateAsync
                        |> Async.Parallel    
                    match templates with
                    | HaveConflicts err ->
                        return Faulted err
                    | _ ->                    
                        let! environmentHearts =
                            templates
                            |> Seq.map toHealthObservableAsync
                            |> Async.Parallel
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