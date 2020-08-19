module EasyDashboard.Domain.Summary.Workflows    
    
    open System
    open Newtonsoft.Json.Linq
    open EasyDashboard.Domain.Template.Models
    open EasyDashboard.Domain.Summary.Models
    
    type ComposeEnvironmentProperty = EnvironmentPropertyTemplate -> JObject -> Result<EnvironmentProperty, string>
    let toEnvironmentProperty: ComposeEnvironmentProperty =
        fun propertyTemplate (jObject: JObject) ->
            let property = { Name = propertyTemplate.Name
                             SourceUrl }
            match jObject.SelectToken(propertyTemplate.Path) with
            | null ->
                { Name = propertyTemplate.Name }
            | 
            
    
    
    type FetchEnvironmentJson = Uri -> Result<string, JObject>
    type ComposeEnvironmentSummary = FetchEnvironmentJson -> EnvironmentTemplate -> Result<EnvironmentSummary, string>
    let toEnvironmentSummary: ComposeEnvironmentSummary =
        fun fetchEnvironmentTemplate template ->
            let templateJson - 
