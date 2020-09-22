module EasyDashboard.CheckEngine.Workflows.InitHealthCheck.Adapters

    open EasyDashboard.Domain.Environment.HeartBeat.Ports
    open EasyDashboard.Domain.Environment.Template.Models.Url

    open System.Net.Http
    
    type EndpointDataAsyncProviderFactory = IHttpClientFactory -> CallEndpointAsync
    
    let createGetHttpRequest (url: Url) =
        new HttpRequestMessage(HttpMethod.Get, (value url))       
    
    let getEndpointDataAsyncProvider: EndpointDataAsyncProviderFactory =
        fun factory ->
            let httpClient = factory.CreateClient()
            
            fun url ->
                async {
                    try
                        let! response =
                            url
                            |> createGetHttpRequest
                            |> httpClient.SendAsync
                            |> Async.AwaitTask
                            
                        if response.IsSuccessStatusCode then
                            let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
                            return SuccessfulResponse {
                                Url = url
                                Content = content
                            }
                        else
                            return NonSuccessfulResponse {
                                Url = url
                                ResponseCode = response.StatusCode
                                ResponseMessage = response.ReasonPhrase
                            }
                    with
                    | exn ->
                        return RequestExecutionError {
                            Url = url
                            Error = exn.ToString()
                        }                
                }
           