module EasyDashboard.Domain.Template.Provider

    type GetTemplatesCommand = {
        FolderPath: string
    }
    type RequestTemplateCommand = {
        Filename: string
        Read: string Async
    }
    type TemplateContent = {
        Filename: string
        Content : string
    }
    type TemplateRequestError = {
        Filename: string
        Error: string
    }
    type RequestedTemplate =
    | TemplateContent of TemplateContent
    | TemplateError of TemplateRequestError    
    type GetTemplateSequence = GetTemplatesCommand -> Result<RequestedTemplate Async seq option, string>