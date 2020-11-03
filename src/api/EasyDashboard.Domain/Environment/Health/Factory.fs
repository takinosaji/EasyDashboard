module EasyDashboard.Domain.Environment.Health.Factory

    open EasyDashboard.Domain.Environment.Health.Models
    open EasyDashboard.Domain.Environment.HeartBeat.Ports    
    open EasyDashboard.Domain.Environment.Models
    open EasyDashboard.Domain.Environment.Template.Parsing
    
    open Result    
    open System
        
    type EnvironmentHealthCreationError =
        | InvalidName of string
        | InvalidDescription of string
        | InvalidCriterion of string
        | InvalidRefreshInterval of string
        | InvalidProperty of string
        | InvalidUri of string
        static member StringErrorAdapter func ctor =
            fun input ->
                match func input with
                | Ok input -> Ok input
                | Error value -> Error (ctor value)
        member x.ToString() = function
            | InvalidName text
            | InvalidDescription text
            | InvalidCriterion text
            | InvalidRefreshInterval text
            | InvalidProperty text
            | InvalidUri text
                -> text
        
    type CreateFromFaultedTemplate = FaultedTemplate -> Result<EnvironmentHealth, EnvironmentHealthCreationError>
    let createFromFaultedTemplate: CreateFromFaultedTemplate =
        fun faultedTemplate ->
            result {
                let! name = faultedTemplate.Name
                            |> EnvironmentHealthCreationError.StringErrorAdapter Name.create InvalidName
                let! description = faultedTemplate.Error
                                    |> EnvironmentHealthCreationError.StringErrorAdapter
                                           Description.create
                                           InvalidDescription     
                return {
                    Name = name
                    Status = HealthStatus.Dead
                    Description = Some description
                    HeartbeatTime = DateTimeOffset.UtcNow
                    Properties = None
                }
            }
                
    type CreateFromIncorrectTemplate = IncorrectTemplate -> Result<EnvironmentHealth, EnvironmentHealthCreationError>
    let createFromIncorrectTemplate: CreateFromIncorrectTemplate =
        fun incorrectTemplate ->
             incorrectTemplate
                |> FaultedTemplate.FromIncorrectTemplate
                |> createFromFaultedTemplate
      
    type CreateFromEnvironmentHeartBeat = EnvironmentHeartBeat -> Result<EnvironmentHealth, EnvironmentHealthCreationError>  
    let createFromHeartBeat: CreateFromEnvironmentHeartBeat = //TODO
        fun heartBeat ->
            result {
                let! name = "stub"
                            |> EnvironmentHealthCreationError.StringErrorAdapter Name.create InvalidName
                let! description = "stub"
                                    |> EnvironmentHealthCreationError.StringErrorAdapter
                                           Description.create
                                           InvalidDescription    
                return {
                    Name = name
                    Status = HealthStatus.Dead
                    Description = Some description
                    HeartbeatTime = DateTimeOffset.UtcNow
                    Properties = None
                }
            }
         
//    type CreateFromParsedTemplateAsync =
//         ProvideEnvironmentHeartBeatAsync
//            -> ParsedTemplate
//            -> Result<EnvironmentHealth, EnvironmentHealthCreationError> Async       
//    let createFromParsedTemplateAsync: CreateFromParsedTemplateAsync =
//        fun provideEnvironmentHeartBeatAsync parsedTemplate ->
//        async {
//            match parsedTemplate with
//            | Unrecognized failedTemplate -> return failedTemplate |> createFromFaultedTemplate 
//            | WithErrors incorrectTemplate -> return incorrectTemplate |> createFromIncorrectTemplate     
//            | Correct environmentTemplate ->
//                let! heartBeat = environmentTemplate |> provideEnvironmentHeartBeatAsync
//                return heartBeat |> createFromHeartBeat
//        }