module EasyDashboard.Domain.Health.Models

    open System

    type HealthStatus =
        | Healthy
        | Unhealthy
        | Sick
    
    type HealthProperty = {
        Name: string
        Description: string
        Value: string
        SourceUrl: string
        Status: HealthStatus
    }
        
    type EnvironmentHeartBeat = {
        Name: string
        Status: HealthStatus
        HeartbeatTime: DateTimeOffset
        Properties: HealthProperty list
    }
    
    type HealthDashboard = {
        Environments: EnvironmentHeartBeat list
    }