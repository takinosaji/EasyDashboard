module EasyDashboard.Domain.Environment.Template.Parsing

    open EasyDashboard.Domain.Environment.Template.Factory
    open EasyDashboard.Domain.Environment.Template.Factory.DTOs
    open EasyDashboard.Domain.Environment.Template.Models
    open EasyDashboard.Domain.Environment.Template.Ports
    
    open Newtonsoft.Json

    type IncorrectTemplate = {
        Name: string
        Error: EnvironmentTemplateCreationError
    }
    type FaultedTemplate = {
        Name: string
        Error: string
    } with
        static member FromIncorrectTemplate (t: IncorrectTemplate) = 
            match t.Error with
            | InvalidName text
            | InvalidDescription text
            | InvalidCriterion text
            | InvalidRefreshInterval text
            | InvalidProperty text
            | InvalidUri text
             -> {
                 Name = t.Name
                 Error = text
                 }
            
    type ParsedTemplate =
        | Correct of EnvironmentTemplate
        | WithErrors of IncorrectTemplate
        | Unrecognized of FaultedTemplate
    type ProcessedTemplate =
        | Parsed of ParsedTemplate
        | Faulted of FaultedTemplate
    
    type ParseTemplate = AcquiredTemplateContent -> ParsedTemplate
    let parseTemplate: ParseTemplate =
        fun template ->
            try
                let templateDto = JsonConvert.DeserializeObject<EnvironmentTemplateDto>(template.Content)
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