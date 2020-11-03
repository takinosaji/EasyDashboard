module EasyDashboard.CheckEngine.Workflows.InitHealthCheck.HealthObservableProvider

    open EasyDashboard.Domain.Environment.Health.Factory
    open EasyDashboard.Domain.Environment.Health.Models
    open EasyDashboard.Domain.Environment.HeartBeat.Ports
    open EasyDashboard.Domain.Environment.Template.Models
    open EasyDashboard.Domain.Environment.Template.Parsing

    open System
    open System.Reactive.Subjects
    open System.Reactive.Linq
                               
    type EnvironmentHeartBeatCreationResult =
        Result<EnvironmentHealth, EnvironmentHealthCreationError> IConnectableObservable

    type HealthObservableAsyncProvider =
        ProvideEnvironmentHeartBeatAsync -> ProcessedTemplate -> EnvironmentHeartBeatCreationResult Async
        
    let createMulticastFrom<'T> (observable: 'T IObservable) =
        new BehaviorSubject<'T>(Unchecked.defaultof<'T>)
            :> ISubject<'T, 'T>
            |> observable.Multicast

    let unprocessedTemplateToObservable (unprocessedTemplate: FaultedTemplate) =
        unprocessedTemplate
        |> createFromFaultedTemplate 
        |> Observable.Return
        |> createMulticastFrom
        
    let incorrectTemplateToObservable (incorrectTemplate: IncorrectTemplate) =
        incorrectTemplate
        |> createFromIncorrectTemplate 
        |> Observable.Return
        |> createMulticastFrom
        
    let correctTemplateToObservableAsync //TODO
        (provideEnvironmentHeartBeatAsync: ProvideEnvironmentHeartBeatAsync)
        (correctTemplate: EnvironmentTemplate) =
        async {
            correctTemplate
            |> provideEnvironmentHeartBeatAsync
            |> createFromHeartBeat
            |> Observable.Return
            |> createMulticastFrom
        }
        
    let parsedTemplateToObservableAsync
        (provideEnvironmentHeartBeatAsync: ProvideEnvironmentHeartBeatAsync)
        (parsedTemplate: ParsedTemplate) =
        async {
            match parsedTemplate with
            | Unrecognized faultedTemplate -> return faultedTemplate |> unprocessedTemplateToObservable 
            | WithErrors incorrectTemplate -> return incorrectTemplate |> incorrectTemplateToObservable
            | Correct correctTemplate ->
                return! correctTemplate |> correctTemplateToObservableAsync provideEnvironmentHeartBeatAsync 
        }
                 
    let createEnvironmentHeartAsync: HealthObservableAsyncProvider =
        fun provideEnvironmentHeartBeatAsync processedTemplate ->
            async {
                match processedTemplate with
                | Unprocessed unprocessedTemplate ->
                    return unprocessedTemplateToObservable unprocessedTemplate
                | Parsed parsedTemplate ->
                    return! parsedTemplate |> parsedTemplateToObservableAsync provideEnvironmentHeartBeatAsync  
            }