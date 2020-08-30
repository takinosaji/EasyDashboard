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
        ProcessedTemplate -> EnvironmentHeartBeatCreationResult Async
        
    let createMulticastFrom<'T> (observable: 'T IObservable) =
        new BehaviorSubject<'T>(Unchecked.defaultof<'T>)
            :> ISubject<'T, 'T>
            |> observable.Multicast

    let faultedTemplateToObservable (faultedTemplate: FaultedTemplate) =
        createFromFaultedTemplate faultedTemplate
        |> Observable.Return
        |> createMulticastFrom
        
    let incorrectTemplateToObservable (incorrectTemplate: IncorrectTemplate) =
        createFromIncorrectTemplate incorrectTemplate
        |> Observable.Return
        |> createMulticastFrom
        
    let parsedTemplateToObservableAsync (parsedTemplate: ParsedTemplate) =
        async {
            match parsedTemplate with
            | Unrecognized faultedTemplate -> return faultedTemplate |> faultedTemplateToObservable 
            | WithErrors incorrectTemplate -> return incorrectTemplate |> incorrectTemplateToObservable
            | Correct correctTemplate ->
                ()
        }
                 
    let createEnvironmentHeartAsync: HealthObservableAsyncProvider =
        fun processedTemplate ->
            async {
                let! processedTemplate = processedTemplate
                match processedTemplate with
                | Faulted faultedTemplate ->
                    return faultedTemplateToObservable faultedTemplate
                | Processed parsedTemplate ->
                    return! parsedTemplateToObservableAsync parsedTemplate
            }