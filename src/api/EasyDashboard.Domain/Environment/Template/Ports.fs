module EasyDashboard.Domain.Environment.Template.Ports

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
    type RequestTemplateAction = (unit -> RequestedTemplate Async)
    type TemplateAsyncProvider = GetTemplatesCommand -> Result<RequestTemplateAction seq option, string>