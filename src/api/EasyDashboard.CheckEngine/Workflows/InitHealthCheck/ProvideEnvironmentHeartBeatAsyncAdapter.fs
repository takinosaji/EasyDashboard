namespace EasyDashboard.CheckEngine.Workflows.InitHealthCheck.Adapters

    open EasyDashboard.Domain.Environment.HeartBeat.Ports

    let getEnvronmentHeartBeat: ProvideEnvironmentHeartBeatAsync =
        fun callEndpointAsync template ->
            ()