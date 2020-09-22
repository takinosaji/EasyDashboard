module EasyDashboard.Domain.Environment.Template.Factory

    open EasyDashboard.Domain.Environment.Template.Models
    open EasyDashboard.Domain.Environment.Models
    
    open System
    open Result
       
    module DTOs =
        type HealthCriteriaTemplateDto =
            {
                HealthyCriterion: string
                UnhealthyCriterion: string
            }  
             
        type EnvironmentPropertyTemplateDto =
            {
                Name: string
                Description: string
                Path: string
                HealthCriteria: HealthCriteriaTemplateDto
            }
            
        type EnvironmentEndpointTemplateDto =
            {
                Uri: string
                ContentType: string
                Properties: EnvironmentPropertyTemplateDto list                
            }
        
        type EnvironmentTemplateDto =
            {
                Name: string
                Description: string
                RefreshInterval: int
                Endpoints: EnvironmentEndpointTemplateDto list
            }
        
    type EnvironmentTemplateCreationError =
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
                        
    type ToHealthCriteriaModel = DTOs.HealthCriteriaTemplateDto ->
                                    Result<HealthCriteriaTemplate, EnvironmentTemplateCreationError>
    let toHealthCriteriaModel: ToHealthCriteriaModel =
        fun healthDto ->
            let adaptedCriterionFactory = EnvironmentTemplateCreationError.StringErrorAdapter
                                              HealthCriterion.create
                                              InvalidCriterion
            
            result {
                let! healthyCriterion = healthDto.HealthyCriterion |> adaptedCriterionFactory
                let! unhealthyCriterion =
                    if String.IsNullOrEmpty healthDto.UnhealthyCriterion then
                        Ok None
                    else
                        healthDto.UnhealthyCriterion
                        |> adaptedCriterionFactory
                        |> toOption
                        
                
                return {
                    HealthyCriterion = healthyCriterion
                    UnhealthyCriterion = unhealthyCriterion
                }
            }
                    
                    
    type ToPropertyModel = DTOs.EnvironmentPropertyTemplateDto ->
                            Result<EnvironmentPropertyTemplate, EnvironmentTemplateCreationError>
    let toPropertyModel: ToPropertyModel =
        fun propertyDto ->
            let adaptedPropertyPartFactory smartCtor =
                EnvironmentTemplateCreationError.StringErrorAdapter smartCtor InvalidProperty      
            
            result {
                let! name = propertyDto.Name.Trim() |> adaptedPropertyPartFactory Name.create
                let! description =
                    if String.IsNullOrWhiteSpace propertyDto.Description then
                        Ok None
                    else
                        propertyDto.Description.Trim()
                        |> adaptedPropertyPartFactory Description.create
                        |> toOption
                let! path = propertyDto.Path |> adaptedPropertyPartFactory PropertyPath.create 
                let! healthCriteria = toHealthCriteriaModel propertyDto.HealthCriteria
                
                return {
                    Name = name
                    Description = description
                    Path = path
                    HealthCriteria = healthCriteria
                }
            }

    type ToEndpointModel = DTOs.EnvironmentEndpointTemplateDto ->
                            Result<EnvironmentEndpointTemplate, EnvironmentTemplateCreationError>
    let toEndpointModel: ToEndpointModel =
        fun endpointDto ->
            result {
                let adaptedUriFactory = EnvironmentTemplateCreationError.StringErrorAdapter
                                                  Url.create
                                                  InvalidUri
                      
                let! uri = endpointDto.Uri |> adaptedUriFactory
                let! properties = endpointDto.Properties |> List.map (fun prop -> toPropertyModel prop) |> sequence
                
                return {
                    Url = uri
                    Properties = properties
                }
            }     
    
    type ToEnvironmentModel = DTOs.EnvironmentTemplateDto -> Result<EnvironmentTemplate, EnvironmentTemplateCreationError>
    let toEnvironmentModel: ToEnvironmentModel =
        fun environmentDto ->                
            result {
                let! name = environmentDto.Name.Trim() |>
                            EnvironmentTemplateCreationError.StringErrorAdapter Name.create InvalidName
                let! description = if String.IsNullOrWhiteSpace environmentDto.Description then
                                    Ok None
                                    else 
                                       environmentDto.Description.Trim()
                                        |> EnvironmentTemplateCreationError.StringErrorAdapter
                                           Description.create
                                           InvalidDescription
                                        |> toOption
                let! refreshInterval = environmentDto.RefreshInterval
                                        |> EnvironmentTemplateCreationError.StringErrorAdapter
                                               RefreshInterval.create
                                               InvalidRefreshInterval                   
                let! endpoints = environmentDto.Endpoints |> List.map (fun e -> toEndpointModel e) |> sequence
                
                return {
                    Name = name
                    Description = description
                    RefreshInterval = refreshInterval
                    Endpoints = endpoints
                }                
            }