module EasyDashboard.Domain.Template.Factory

    open EasyDashboard.Domain.Template.Dtos
    open EasyDashboard.Domain.Template.Models
    open EasyDashboard.Domain.BuiltInTypeFactory
    
    open System
    open Result
        
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
        
    type ToHealthCriteriaModel = HealthCriteriaTemplateDto ->
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
                    
                    
    type ToPropertyModel = EnvironmentPropertyTemplateDto ->
                            Result<EnvironmentPropertyTemplate, EnvironmentTemplateCreationError>
    let toPropertyModel: ToPropertyModel =
        fun propertyDto ->
            let adaptedPropertyPartFactory smartCtor =
                EnvironmentTemplateCreationError.StringErrorAdapter smartCtor InvalidProperty      
            
            result {
                let! name = propertyDto.Name |> adaptedPropertyPartFactory Name.create
                let! description =
                    if String.IsNullOrEmpty propertyDto.Description then
                        Ok None
                    else
                        propertyDto.Description
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

    type ToEndpointModel = EnvironmentEndpointTemplateDto ->
                            Result<EnvironmentEndpointTemplate, EnvironmentTemplateCreationError>
    let toEndpointModel: ToEndpointModel =
        fun endpointDto ->
            result {
                let adaptedUriFactory = EnvironmentTemplateCreationError.StringErrorAdapter
                                                  createUri
                                                  InvalidUri
                      
                let! uri = endpointDto.Uri |> adaptedUriFactory
                let! properties = endpointDto.Properties |> List.map (fun e -> toPropertyModel e) |> sequence
                
                return {
                    Url = uri
                    Properties = properties
                }
            }     
    
    type ToEnvironmentModel = EnvironmentTemplateDto -> Result<EnvironmentTemplate, EnvironmentTemplateCreationError>
    let toEnvironmentModel: ToEnvironmentModel =
        fun templateDto ->                
            result {
                let! name = templateDto.Name |>
                            EnvironmentTemplateCreationError.StringErrorAdapter Name.create InvalidName
                let! description = if String.IsNullOrEmpty templateDto.Description then
                                    Ok None
                                    else 
                                       templateDto.Description
                                        |> EnvironmentTemplateCreationError.StringErrorAdapter
                                           Description.create
                                           InvalidDescription
                                        |> toOption
                let! refreshInterval = templateDto.RefreshInterval
                                        |> EnvironmentTemplateCreationError.StringErrorAdapter
                                               RefreshInterval.create
                                               InvalidRefreshInterval                   
                let! endpoints = templateDto.Endpoints |> List.map (fun e -> toEndpointModel e) |> sequence
                
                return {
                    Name = name
                    Description = description
                    RefreshInterval = refreshInterval
                    Endpoints = endpoints
                }                
            }