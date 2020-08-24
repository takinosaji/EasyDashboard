module EasyDashboard.CheckEngine.InitHealthCheck.Workflow
        
    open EasyDashboard.Domain.Template.Dtos
    open EasyDashboard.Domain.Template.Models
    open EasyDashboard.Domain.Template.Factory
    open EasyDashboard.Domain.Template.Provider
    open EasyDashboard.Domain.Health.Models

    open System
    open System.Text.Json;
                              
    type IncorrectTemplate = {
        Name: string
        Error: EnvironmentTemplateCreationError
    }
    type FailedTemplate = {
        Name: string
        Error: string
    }
    type ParsedTemplate =
        | Correct of EnvironmentTemplate
        | WithErrors of IncorrectTemplate
        | Failed of FailedTemplate
    type ProcessedTemplate =
        | Processed of ParsedTemplate
        | Unprocessed of FailedTemplate
    type ParseTemplate = TemplateContent -> ParsedTemplate
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
            | exn -> Failed {
                Name = template.Filename
                Error = exn.ToString()
            }
             
    type ProcessTemplate = RequestedTemplate -> ProcessedTemplate   
    let processTemplate: ProcessTemplate =
        fun requestedTemplate ->
            match requestedTemplate with
            | TemplateError failedRequest ->
                Unprocessed {
                    Name = failedRequest.Filename
                    Error = failedRequest.Error
                }
            | TemplateContent successfulRequest ->
                Processed (parseTemplate successfulRequest)
   
     
            
            
    type InitHeathCheckCommand = {
        FolderPath: string
    }
    type CheckEngineState =
        | Working of EnvironmentHeartBeat IObservable
        | Idling
        | Faulted of string                 
    type InitHealthCheckUnitsAsync = InitHeathCheckCommand -> GetTemplateSequence -> CheckEngineState Async seq   
    let initHealthCheckUnitsAsync: InitHealthCheckUnitsAsync =
        fun initCommand templateProvider ->
            async {
               let templateResult = templateProvider { FolderPath = initCommand.FolderPath }
               match templateResult with
               | Error err -> return Faulted err
               | Ok None -> return Idling
               | Ok (Some templateSequence) ->
                   let! templates = templateSequence |> Async.Parallel
                   let c = templates
                         |> Array.map (fun template -> processTemplate template)
                   
            }
            
            
            
            
            
            
            
            
            
            
            
            
    type WrapInObservable = EnvironmentTemplate -> EnvironmentHeartBeat IObservable            
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