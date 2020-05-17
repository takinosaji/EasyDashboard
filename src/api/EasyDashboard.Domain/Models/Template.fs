module Models.Template

type HealthCriteriaTemplate =
    {
        HealthyCriteria: string
        UnhealthyCriteria: string option         
    }

type PropertyTemplate =
    {
        PropertyName: string
        PropertyDescription: string
        PropertyPath: string
        HealthCriteria: HealthCriteriaTemplate
    }

type EnvironmentEndpointTemplate =
    {
        Url: string
        Properties: PropertyTemplate list                
    }

type EnvironmentTemplate =
    {
        RefreshInterval: int
        Endpoints: EnvironmentEndpointTemplate list
    }