module EasyDashboard.CheckEngine.InitHealthCheck.Workflow
        
    open EasyDashboard.CheckEngine.Workflows.InitHealthCheck.HealthObservableProvider
    open EasyDashboard.Domain.Environment.HeartBeat.Ports
    open EasyDashboard.Domain.Environment.Template.Parsing
    open EasyDashboard.Domain.Environment.Template.Ports
    open EasyDashboard.Domain.Environment.Template.CollectionValidation
                 
    type ProcessTemplateRequestAsync = TemplateAsyncRequest -> ProcessedTemplate Async  
    let processTemplateRequestAsync: ProcessTemplateRequestAsync =
        fun requestTemplateAsync ->
            async {
                let! requestedTemplate = requestTemplateAsync()
                match requestedTemplate with
                | TemplateError failedRequest ->
                    return Unprocessed {
                        Name = failedRequest.Filename
                        Error = failedRequest.Error
                    }
                | RequestedTemplate successfulRequest ->
                    return Parsed (parseTemplate successfulRequest)    
             }
        
    type InitHeathCheckCommand = {
        FolderPath: string
    }
    type CheckEngineState =
        | Initializing
        | Working of EnvironmentHeartBeatCreationResult seq
        | Idling
        | Faulted of string                 
    type InitHealthCheckEngineAsync =
         ProvideTemplateRequestAsync -> CallEndpointAsync -> InitHeathCheckCommand -> CheckEngineState Async 
    let initHealthCheckEngineAsync: InitHealthCheckEngineAsync =
        fun provideTemplatesAsync callEndpointAsync initCommand  ->
            
            let getEnvironmentHeartBeatAsync = getEnvironmentHeartBeatAsync callEndpointAsync
            let createEnvironmentHeartAsync = createEnvironmentHeartAsync getEnvironmentHeartBeatAsync
            
            async {
               let availableTemplateRequests = provideTemplatesAsync { FolderPath = initCommand.FolderPath }
               match availableTemplateRequests with
               | Error err -> return Faulted err
               | Ok None -> return Idling
               | Ok (Some templateRequests) ->
                    let! templates =
                        templateRequests 
                        |> Seq.map processTemplateRequestAsync
                        |> Async.Parallel    
                    match templates with
                    | HaveConflicts conflict ->
                        return Faulted conflict
                    | _ ->                    
                        let! environmentHearts =
                            templates
                            |> Array.map createEnvironmentHeartAsync
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