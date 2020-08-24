module EasyDashboard.Domain.Template.Models

    open System
    open EasyDashboard.Domain.ConstrainedType
    
        module HealthCriterion =
            type HealthCriterion = private HealthCriterion of string    
            let create criterion =
                createNotEmptyString (nameof HealthCriterion) HealthCriterion criterion         
            let value (HealthCriterion criterion) = criterion

        module Name =
            type Name = private Name of string 
            let create name =
                createLimitedString (nameof Name) Name 25 name           
            let value (Name name) = name
          
          
            
        module Description =
            type Description = private Description of string 
            let create description =
                createLimitedString (nameof Description) Description 500 description           
            let value (Description description) = description
         
         
         
        module PropertyPath =
            type PropertyPath = private PropertyPath of string 
            let create path =
                createLimitedString (nameof PropertyPath) PropertyPath 500 path           
            let value (PropertyPath path) = path   
            
            
            
        module RefreshInterval =   
            type RefreshInterval = private RefreshInterval of int   
            let create interval =
                createInt  (nameof RefreshInterval) RefreshInterval 10 300 interval            
            let value (RefreshInterval interval) = interval
    
        type HealthCriteriaTemplate =
            {
                HealthyCriterion: HealthCriterion.HealthCriterion
                UnhealthyCriterion: HealthCriterion.HealthCriterion option
            }    
       
        type EnvironmentPropertyTemplate =
            {
                Name: Name.Name
                Description: Description.Description option
                Path: PropertyPath.PropertyPath
                HealthCriteria: HealthCriteriaTemplate
            }
            
        type EnvironmentEndpointTemplate =
            {
                Url: Uri
                Properties: EnvironmentPropertyTemplate list                
            }

        type EnvironmentTemplate =
            {
                Name: Name.Name
                Description: Description.Description option
                RefreshInterval: RefreshInterval.RefreshInterval
                Endpoints: EnvironmentEndpointTemplate list
            }

        
    