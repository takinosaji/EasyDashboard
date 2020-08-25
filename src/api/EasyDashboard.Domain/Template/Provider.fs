module EasyDashboard.Domain.Template.Provider

    type GetTemplatesCommand = {
        FolderPath: string
    }
    type TemplateContentDto = {
        Filename: string
        Content : string
    }
    type TemplateRequestErrorDto = {
        Filename: string
        Error: string
    }
    type RequestedTemplateDto =
    | TemplateContent of TemplateContentDto
    | TemplateError of TemplateRequestErrorDto    
    type TemplatesProvider = GetTemplatesCommand -> Result<RequestedTemplateDto Async seq option, string>