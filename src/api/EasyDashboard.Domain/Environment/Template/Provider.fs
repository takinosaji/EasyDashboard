module EasyDashboard.Domain.Environment.Template.Provider

    type GetTemplatesCommand = {
        FolderPath: string
    }
    type AcquiredTemplateContent = {
        Filename: string
        Content : string
    }
    type TemplateRequestError = {
        Filename: string
        Error: string
    }
    type RequestedTemplate =
    | TemplateContent of AcquiredTemplateContent
    | TemplateError of TemplateRequestError    
    type TemplateAsyncProvider = GetTemplatesCommand -> Result<RequestedTemplate Async seq option, string>