module EasyDashboard.Domain.Template.Dtos

    type EnvironmentPropertyTemplateDto =
        {
            Name: string
            Description: string
            Path: string
            HealthCriteria: string
        }
     
    type EnvironmentEndpointTemplateDto =
        {
            Url: string
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
        
