module EasyDashboard.Domain.Environment.HeartBeat.Factory

    open EasyDashboard.Domain.Environment.HeartBeat.Models
    open EasyDashboard.Domain.Environment.HeartBeat.Ports    
    open EasyDashboard.Domain.Environment.Models
    open EasyDashboard.Domain.Environment.Template.Models    
    open EasyDashboard.Domain.Environment.Template.Parsing
    
    open Result    
    open System
        
    type EnvironmentHeartCreationError =
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
    
    type CreateEnvironmentHeartBeatCommand = {
        Template: EnvironmentTemplate
        Data: EndpointResponse
    }
    
    type CreateFromFaultedTemplate = FaultedTemplate -> Result<EnvironmentHeartBeat, EnvironmentHeartCreationError>
    let createFromFaultedTemplate: CreateFromFaultedTemplate =
        fun faultedTemplate ->
            result {
                let! name = faultedTemplate.Name
                            |> EnvironmentHeartCreationError.StringErrorAdapter Name.create InvalidName
                let! description = faultedTemplate.Error
                                    |> EnvironmentHeartCreationError.StringErrorAdapter
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
                
    type CreateFromIncorrectTemplate = IncorrectTemplate -> Result<EnvironmentHeartBeat, EnvironmentHeartCreationError>
    let createFromIncorrectTemplate: CreateFromIncorrectTemplate =
        fun incorrectTemplate ->
             incorrectTemplate
                |> FaultedTemplate.FromIncorrectTemplate
                |> createFromFaultedTemplate
      
    //type CreateFromEnvironmentTemplate = EnvironmentHealthProvider -> EnvironmentTemplate -> Result<EnvironmentHeartBeat, EnvironmentHeartBeatCreationError>  
    type CreateFromEnvironmentTemplate = CreateEnvironmentHeartBeatCommand -> Result<EnvironmentHeartBeat, EnvironmentHeartCreationError>  
    let createFromCorrectTemplate: CreateFromEnvironmentTemplate =
        fun command ->
            command.Data
            ()
        
    
    
    type CreateFromParsedTemplate = ParsedTemplate -> Result<EnvironmentHeartBeat, EnvironmentHeartCreationError>       
    let createFromParsedTemplate (parsedTemplate: ParsedTemplate) =
        match parsedTemplate with
        | Unrecognized failedTemplate -> failedTemplate |> createFromFaultedTemplate 
        | WithErrors incorrectTemplate -> incorrectTemplate |> createFromIncorrectTemplate     
        | Correct environmentTemplate -> environmentTemplate |> createFromCorrectTemplate
          

//    open System
//    open Newtonsoft.Json.Linq
//    open EasyDashboard.Domain.Template.Models
//    open EasyDashboard.Domain.Health.Models
//    
//    type ComposeEnvironmentProperty = EnvironmentPropertyTemplate -> JObject -> Result<HealthProperty, string>
////    let toEnvironmentProperty: ComposeEnvironmentProperty =
////        fun propertyTemplate (jObject: JObject) ->
////            let property = { Name = propertyTemplate.Name
////                             SourceUrl }
////            match jObject.SelectToken(propertyTemplate.Path) with
////            | null ->
////                { Name = propertyTemplate.Name }
////            | 
////            
////    
////    
////    type FetchEnvironmentJson = Uri -> Result<string, JObject>
////    type ComposeEnvironmentSummary = FetchEnvironmentJson -> EnvironmentTemplate -> Result<EnvironmentSummary, string>
////    let toEnvironmentSummary: ComposeEnvironmentSummary =
////        fun fetchEnvironmentTemplate template ->
////            let templateJson - 
