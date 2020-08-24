module EasyDashboard.Api.Settings

    open Microsoft.Extensions.Configuration
    
    let [<Literal>] private ConfigSectionName = "Kestrel"        
    let [<Literal>] private HomeDirectoryField = "UiDirectory"

    type ApiStartupSettings = {
        UiDirectory: string
    }
        
    let extractStartupSettings (configuration: IConfiguration) =
        let hostingSection = configuration.GetSection ConfigSectionName
        {
            UiDirectory = hostingSection.GetValue(HomeDirectoryField)
        }