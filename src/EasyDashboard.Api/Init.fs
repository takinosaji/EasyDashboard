﻿namespace EasyDashboard

open System.Threading
open Suave

module Api =
    let Init host port =
          let cts = new CancellationTokenSource()
          let conf = { defaultConfig
                       with cancellationToken = cts.Token; bindings = [ HttpBinding.createSimple HTTP host port ] }
          
          let listening, server = startWebServerAsync conf (Successful.OK "Hello World")
            
          Async.Start(server, cts.Token)       
          cts