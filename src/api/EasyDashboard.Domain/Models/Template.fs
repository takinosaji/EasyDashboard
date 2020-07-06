module Models.Template

type HealthyCriteria = string
type UnhealthyCriteria = string

type HealthCriteriaTemplate =
    {
        HealthyCriteria: HealthyCriteria
        UnhealthyCriteria: UnhealthyCriteria option
    }

type PropertyName = string
type PropertyDescription = string
type PropertyPath = string

type PropertyTemplate =
    {
        Name: PropertyName
        Description: PropertyDescription
        Path: PropertyPath
        HealthCriteria: HealthCriteriaTemplate
    }

type Url = string

type EnvironmentEndpointTemplate =
    {
        Url: Url
        Properties: PropertyTemplate list                
    }

type RefreshInterval = int

type EnvironmentTemplate =
    {
        RefreshInterval: RefreshInterval
        Endpoints: EnvironmentEndpointTemplate list
    }