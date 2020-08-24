module EasyDashboard.Domain.BuiltInTypeFactory

    open System
    
    type CreateUri = string -> Result<Uri, string>
    let createUri: CreateUri =
        fun uri ->
            try
                Ok (Uri uri)
            with
                exn -> Error(exn.ToString())