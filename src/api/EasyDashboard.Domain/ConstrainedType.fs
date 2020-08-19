module EasyDashboard.Domain.ConstrainedType

    open System

    let createNotEmptyString fieldName ctor str = 
        if String.IsNullOrEmpty(str) then
            let msg = sprintf "%s must not be null or empty" fieldName 
            Error msg
        else
            Ok (ctor str)
                

    let createLimitedString fieldName ctor maxLen str = 
        if String.IsNullOrEmpty(str) then
            let msg = sprintf "%s must not be null or empty" fieldName 
            Error msg
        elif str.Length > maxLen then
            let msg = sprintf "%s must not be more than %i chars" fieldName maxLen 
            Error msg 
        else
            Ok (ctor str)


    let createStringOption fieldName ctor maxLen str = 
        if String.IsNullOrEmpty(str) then
            Ok None
        elif str.Length > maxLen then
            let msg = sprintf "%s must not be more than %i chars" fieldName maxLen 
            Error msg 
        else
            Ok (ctor str |> Some)


    let createInt fieldName ctor minVal maxVal i = 
        if i < minVal then
            let msg = sprintf "%s: Must not be less than %i" fieldName minVal
            Error msg
        elif i > maxVal then
            let msg = sprintf "%s: Must not be greater than %i" fieldName maxVal
            Error msg
        else
            Ok (ctor i)


    let createDecimal fieldName ctor minVal maxVal i = 
        if i < minVal then
            let msg = sprintf "%s: Must not be less than %M" fieldName minVal
            Error msg
        elif i > maxVal then
            let msg = sprintf "%s: Must not be greater than %M" fieldName maxVal
            Error msg
        else
            Ok (ctor i)


    let createLike fieldName  ctor pattern str = 
        if String.IsNullOrEmpty(str) then
            let msg = sprintf "%s: Must not be null or empty" fieldName 
            Error msg
        elif System.Text.RegularExpressions.Regex.IsMatch(str,pattern) then
            Ok (ctor str)
        else
            let msg = sprintf "%s: '%s' must match the pattern '%s'" fieldName str pattern
            Error msg 