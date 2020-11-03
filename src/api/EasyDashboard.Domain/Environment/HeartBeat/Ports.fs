module EasyDashboard.Domain.Environment.HeartBeat.Ports

    open EasyDashboard.Domain.Environment.Template.Models
    open EasyDashboard.Domain.Environment.Template.Models.Url
        
    open System.Net
    
    type EndpointContentDto = {
        Url: Url
        Content: string
    }
    
    type EndpointBadResponseDto = {
        Url: Url
        ResponseMessage: string
        ResponseCode: HttpStatusCode
    }
    
    type EndpointExecutionErrorDto = {
        Url: Url
        Error: string
    }
    
    type EndpointResponse =
        | SuccessfulResponse of EndpointContentDto 
        | NonSuccessfulResponse of EndpointBadResponseDto         
        | RequestExecutionError of EndpointExecutionErrorDto
        
    type CallEndpointAsync = Url -> EndpointResponse Async
    
    type EnvironmentHeartBeat = {
        Template: EnvironmentTemplate
        Data: EndpointResponse list
    }        
    
    type ProvideEnvironmentHeartBeatAsync = CallEndpointAsync -> EnvironmentTemplate -> EnvironmentHeartBeat Async