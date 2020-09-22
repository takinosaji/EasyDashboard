module EasyDashboard.Domain.Environment.Template.Ports

    type GetTemplatesCommand = {
        FolderPath: string
    }
    type AcquiredTemplate = {
        Filename: string
        Content : string
    }
    type TemplateRequestError = {
        Filename: string
        Error: string
    }
    
    type TemplateResponse =
    | RequestedTemplate of AcquiredTemplate
    | TemplateError of TemplateRequestError
    type TemplateAsyncRequest = (unit -> TemplateResponse Async)
    type ProvideTemplateRequestAsync = GetTemplatesCommand -> Result<TemplateAsyncRequest seq option, string>