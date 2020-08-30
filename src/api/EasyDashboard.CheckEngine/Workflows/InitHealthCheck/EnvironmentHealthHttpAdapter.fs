module EasyDashboard.CheckEngine.Workflows.InitHealthCheck.EnvironmentHealthHttpProvider

    open EasyDashboard.Domain.Environment.HeartBeat.Ports
    open EasyDashboard.Domain.Environment.Template.Models
    open EasyDashboard.Domain.Environment.Template.Models.Url

    open System.Net.Http
    
    let createGetHttpRequest (url: Url) =
        new HttpRequestMessage(HttpMethod.Get, (Url.value url))
    
    type EndpointDataAsyncProvider = IHttpClientFactory -> Url -> EndpointResponse Async
    let getEnvironmentHealthAsync: EndpointDataAsyncProvider =
        fun factory url ->
            async {
                try
                    let! response =
                        url
                        |> createGetHttpRequest     
                        |> factory.CreateClient().SendAsync
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