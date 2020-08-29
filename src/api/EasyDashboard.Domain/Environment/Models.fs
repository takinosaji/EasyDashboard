module EasyDashboard.Domain.Environment.Models

    open EasyDashboard.Domain.ConstrainedTypes
    
    module Name =
            type Name = private Name of string 
            let create name =
                createLimitedString (nameof Name) Name 25 name           
            let value (Name name) = name           
            
    module Description =
        type Description = private Description of string 
        let create description =
            createLimitedString (nameof Description) Description 500 description           
        let value (Description description) = description