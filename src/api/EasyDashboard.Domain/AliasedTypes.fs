module EasyDashboard.Domain.AliasedTypes

    type AsyncResult<'a, 'e> = Async<Result<'a, 'e>>