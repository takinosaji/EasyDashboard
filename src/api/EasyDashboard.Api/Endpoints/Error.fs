module EasyDashboard.Api.Endpoints.Error

    open System
    open Microsoft.Extensions.Logging
    open Giraffe
    
    let handler (ex : Exception) (logger : ILogger) =
        logger.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")
        clearResponse >=> setStatusCode 500 >=> text ex.Message