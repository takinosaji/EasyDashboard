module EasyDashboard.Domain.Health.Workflows    
//    
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
