module EasyDashboard.Domain.Environment.Health.Models

    open EasyDashboard.Domain.Environment.Models
    
    open System
//
//    Need dtos for factory???
    
    type HealthStatus =
        | Healthy
        | Sick
        | Dead
    
    type HealthProperty = {
        Name: string
        Description: string
        Value: string
        SourceUrl: string
        Status: HealthStatus
    }
        
    type EnvironmentHealth = {
        Name: Name.Name
        Description: Description.Description option
        Status: HealthStatus
        HeartbeatTime: DateTimeOffset
        Properties: HealthProperty list option
    }
    
    type HealthDashboard = {
        Environments: EnvironmentHealth list
    }