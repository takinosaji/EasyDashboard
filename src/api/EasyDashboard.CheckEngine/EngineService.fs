﻿namespace EasyDashboard.CheckEngine

    open Microsoft.Extensions.Hosting
    open Microsoft.Extensions.Logging
    open Microsoft.Extensions.Configuration
            
    type EngineService(logger : ILogger<EngineService>,
                       configuration: IConfiguration) =
        inherit BackgroundService()
        
        let _logger = logger
        
        override bs.ExecuteAsync _ =
            let asyncExpression = async {
                while true do
                    do! Async.Sleep 1000
                    let c = 1
                    ()
                return 0
            }
            
            upcast Async.StartAsTask asyncExpression