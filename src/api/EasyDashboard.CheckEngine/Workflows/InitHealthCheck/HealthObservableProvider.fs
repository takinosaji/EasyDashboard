module EasyDashboard.CheckEngine.Workflows.InitHealthCheck.HealthObservableProvider

    open EasyDashboard.Domain.Environment.HeartBeat.Models
    open EasyDashboard.Domain.Environment.HeartBeat.Factory
    open EasyDashboard.Domain.Environment.Template.Factory

    open System
    open System.Reactive.Subjects
    open System.Reactive.Linq
                               
    type EnvironmentHeartBeatCreationResult =
        Result<EnvironmentHeartBeat, EnvironmentHeartCreationError> IConnectableObservable

    type HealthObservableAsyncProvider =
        ProcessedTemplate Async -> EnvironmentHeartBeatCreationResult Async
        
    let createMulticastFrom<'T> (observable: 'T IObservable) =
        new BehaviorSubject<'T>(Unchecked.defaultof<'T>)
            :> ISubject<'T, 'T>
            |> observable.Multicast

    let faultedTemplateToObservable (faultedTemplate: FaultedTemplate) =
        createFromFaultedTemplate faultedTemplate
        |> Observable.Return
        |> createMulticastFrom
        
    let parsedTemplateToObservable (parsedTemplate: ParsedTemplate) =
        match parsedTemplate with
        | Unrecognized faultedTemplate -> faultedTemplate |> faultedTemplateToObservable 
        | WithErrors incorrectTemplate ->
            incorrectTemplate
                |> FaultedTemplate.FromIncorrectTemplate
                |> faultedTemplateToObservable
        | Correct correctTemplate ->
            ()
                 
    let createEnvironmentHeartAsync: HealthObservableAsyncProvider =
        fun processedTemplateAsync ->
            async {
                let! processedTemplate = processedTemplateAsync
                match processedTemplate with
                | Faulted faultedTemplate ->
                    return faultedTemplateToObservable faultedTemplate
                | Processed parsedTemplate ->
                    return parsedTemplateToObservable parsedTemplate
            }