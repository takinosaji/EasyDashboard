module EasyDashboard.Domain.Environment.Template.Models

    open EasyDashboard.Domain.ConstrainedTypes
    open EasyDashboard.Domain.Environment.Models
    
    module HealthCriterion =
        type HealthCriterion = private HealthCriterion of string    
        let create criterion =
            createNotEmptyString (nameof HealthCriterion) HealthCriterion criterion         
        let value (HealthCriterion criterion) = criterion
   
     
     
     
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

      
        
    module Uri =
        type Uri = private Uri of System.Uri   
        let create (uri: string) =                
            try
                Ok(Uri(System.Uri(uri)))
            with
                exn -> Error(exn.ToString())      
        let value (Uri uri) = uri
    
    
    
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
            Url: Uri.Uri
            Properties: EnvironmentPropertyTemplate list                
        }

    type EnvironmentTemplate =
        {
            Name: Name.Name
            Description: Description.Description option
            RefreshInterval: RefreshInterval.RefreshInterval
            Endpoints: EnvironmentEndpointTemplate list
        }

        
    