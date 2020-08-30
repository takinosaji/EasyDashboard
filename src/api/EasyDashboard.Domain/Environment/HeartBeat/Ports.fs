module EasyDashboard.Domain.Environment.HeartBeat.Ports

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
    
    type EndpointErrorDto = {
        Url: Url
        Error: string
    }
    
    type EndpointResponse =
        | SuccessfulResponse of EndpointContentDto 
        | NonSuccessfulResponse of EndpointBadResponseDto         
        | RequestExecutionError of EndpointErrorDto
        
    type EndpointDataAsyncProvider = Url -> EndpointResponse Async
//    
//        type EndpointContent = {
//        Endpoint: EnvironmentEndpointTemplate
//        Content: string
//    }
//    
//    type EndpointError = {
//        Endpoint: EnvironmentEndpointTemplate
//        Error: string
//    }
//    
//    type EnvironmentEndpointResponse =
//        | SuccessfulResponse of EndpointContent 
//        | EndpointError of EndpointError 
//
//    
//    type FetchUrlAsync = Uri -> string Async
//    
//    
//    type EndpointDataProviderAsync = FetchUrlAsync -> EnvironmentEndpointTemplate -> EnvironmentEndpointResponse Async