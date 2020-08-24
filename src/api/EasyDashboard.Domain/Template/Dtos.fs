module EasyDashboard.Domain.Template.Dtos
        
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