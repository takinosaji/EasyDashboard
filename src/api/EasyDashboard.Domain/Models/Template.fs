module Models.Template

    type HealthyCriteria = HealthyCriteria of string
    type UnhealthyCriteria = UnhealthyCriteria of string

    type HealthCriteriaTemplate =
        {
            HealthyCriteria: HealthyCriteria
            UnhealthyCriteria: UnhealthyCriteria option
        }

    type PropertyName = PropertyName of string
    type PropertyDescription = PropertyDescription of string
    type PropertyPath = PropertyPath of string

    type PropertyTemplate =
        {
            Name: PropertyName
            Description: PropertyDescription
            Path: PropertyPath
            HealthCriteria: HealthCriteriaTemplate
        }

    type Url = Url of string

    type EnvironmentEndpointTemplate =
        {
            Url: Url
            Properties: PropertyTemplate list                
        }

    type RefreshInterval = RefreshInterval of int

    type EnvironmentTemplate =
        {
            RefreshInterval: RefreshInterval
            Endpoints: EnvironmentEndpointTemplate list
        }