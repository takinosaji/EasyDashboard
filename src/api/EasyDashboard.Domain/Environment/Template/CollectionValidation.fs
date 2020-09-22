module EasyDashboard.Domain.Environment.Template.CollectionValidation
 
    open System
    open EasyDashboard.Domain.Environment.Models
    open EasyDashboard.Domain.Environment.Template.Parsing
         
    // TODO: Substitute active pattern with regular function when this logic will have to become injectable   
    let (|HaveConflicts|_|) (templates: ProcessedTemplate[]) =
        let templateNames =
            templates
            |> Array.choose
                (fun t ->
                    match t with
                    | Parsed parsedTemplate ->
                        match parsedTemplate with
                        | Correct c -> Some (Name.value c.Name)
                        | WithErrors e -> Some e.Name
                        | _ -> None
                    | _ -> None)
        let duplicatedNames =
            templateNames
            |> Array.groupBy (fun f -> f)
            |> Array.filter (fun g -> (snd g).Length > 1)
            |> Array.choose (fun g -> Some (fst g))
        match duplicatedNames with
        | [||] -> None
        | _ ->
            let duplicates = String.Join(", ", duplicatedNames)
            Some (sprintf "Templates cannot have repeating names. Duplicates: %s" duplicates)