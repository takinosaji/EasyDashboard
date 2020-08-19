module EasyDashboard.Domain.Versioning

    open System.Reflection
    
    let ApiVersion =
        let assembly = Assembly.GetEntryAssembly()
        let version = assembly.GetName().Version
        sprintf "%i.%i.%i" version.Major version.Minor version.Build
        
    let UiVersion =
        let assembly = Assembly.GetEntryAssembly()
        let version = assembly.GetName().Version
        sprintf "%i.%i.%i" version.Major version.Minor version.Build