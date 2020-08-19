module EasyDashboard.Domain.Summary.Models

    open System

    type EnvironmentHealth =
        | Healthy
        | Unhealthy
        | Sick
    
    type EnvironmentProperty = {
        Name: string
        Description: string
        Value: string
        SourceUrl: string
        Status: EnvironmentHealth
    }
        
    type EnvironmentSummary = {
        Name: string
        Status: EnvironmentHealth
        HeartbeatTime: DateTimeOffset
    }
    
    type DashboardSummary = {
        Environments: EnvironmentHealth list
    }