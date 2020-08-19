module EasyDashboard.Domain.Template.Models

    open System
    open EasyDashboard.Domain.ConstrainedType
        
    module HealthyCriterion =
        type HealthyCriterion = private HealthyCriterion of string    
        let create criterion =
            createNotEmptyString (nameof HealthyCriterion) HealthyCriterion criterion         
        let value (HealthyCriterion criterion) = criterion
    
    
        
    module UnhealthyCriterion =
        type UnhealthyCriterion = private UnhealthyCriterion of string 
        let create criterion =
            createNotEmptyString (nameof UnhealthyCriterion) UnhealthyCriterion criterion           
        let value (UnhealthyCriterion criterion) = criterion

    
    
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
            HealthyCriterion: HealthyCriterion.HealthyCriterion
            UnhealthyCriterion: UnhealthyCriterion.UnhealthyCriterion option
        }    
   
    type EnvironmentPropertyTemplate =
        {
            Name: Name.Name
            Description: Description.Description option
            Path: PropertyPath.PropertyPath
            HealthCriteria: HealthCriteriaTemplate
        }

    type ContentType =
        | Json
        | Xml
        
    type EnvironmentEndpointTemplate =
        {
            Url: Uri
            ContentType: ContentType 
            Properties: EnvironmentPropertyTemplate list                
        }

    type EnvironmentTemplate =
        {
            Name: Name.Name
            Description: Description.Description option
            RefreshInterval: RefreshInterval.RefreshInterval
            Endpoints: EnvironmentEndpointTemplate list
        }

        
    