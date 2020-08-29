module EasyDashboard.Domain.Environment.HeartBeat.Provider

    open EasyDashboard.Domain.Environment.Template.Models.Uri
    
    open Newtonsoft.Json.Linq

    type EnvironmentHealthProvider = Uri -> JObject 