namespace Day03

[<AutoOpen>]
module Functions =
    let isDigit c = c >= '0' && c <= '9'

    let isSymbol c = not (isDigit c) && c <> '.'

    let isAdjacentToSymbol (schematic: EngineSchematic) i j =
        let mutable adjacentTo = None

        let setAdjacentTo row col =
            if adjacentTo.IsSome then
                failwith "More than one symbol adjacent to a number"

            adjacentTo <-
                Some {
                    text = schematic[row][col]
                    position = { row = row; col = col }
                }
        // Check above
        if i > 0 then
            let row = schematic.[i - 1]

            for k in (max 0 (j - 1)) .. (min (row.Length - 1) (j + 1)) do
                if isSymbol row.[k] then
                    setAdjacentTo (i - 1) k
        // Check below
        if i < schematic.Length - 1 then
            let row = schematic.[i + 1]

            for k in (max 0 (j - 1)) .. (min (row.Length - 1) (j + 1)) do
                if isSymbol row.[k] then
                    setAdjacentTo (i + 1) k
        // Check left
        if j > 0 && isSymbol schematic.[i].[j - 1] then
            setAdjacentTo i (j - 1)
        // Check right
        if j < schematic.[i].Length - 1 && isSymbol schematic.[i].[j + 1] then
            setAdjacentTo i (j + 1)

        adjacentTo

    let parseSchematic (schematic: EngineSchematic) : EngineStructure =
        let mutable numbers = []

        let mutable symbol = None

        let setSymbol value =
            match symbol with
            | Some symbolValue when value = symbolValue -> ()
            | Some _ -> failwith "More than one symbol adjacent to a number"
            | None -> symbol <- Some value

        let mutable currentNumber = ""

        for i in 0 .. schematic.Length - 1 do
            for j in 0 .. schematic.[i].Length - 1 do
                if isDigit schematic.[i].[j] then
                    currentNumber <- currentNumber + string schematic.[i].[j]

                    match isAdjacentToSymbol schematic i j with
                    | Some newSymbol -> setSymbol newSymbol
                    | None -> ()
                else if currentNumber <> "" then
                    match symbol with
                    | Some symbol -> numbers <- (uint currentNumber, symbol) :: numbers
                    | None -> ()

                    symbol <- None
                    currentNumber <- ""

        List.rev numbers

    let getNumbersAdjacentToSymbol (structure: EngineStructure) = List.map fst structure

    let getGearRatio (gear: Gear) =
        let x, y = gear
        x * y

    let getGears (structure: EngineStructure) =
        structure
        |> List.groupBy snd
        |> List.choose (fun (symbol, gears) ->
            match symbol.text, gears with
            | '*', [ nr1, _; nr2, _ ] -> Some(nr1, nr2)
            | _ -> None)

    let getGearRatioSum (structure: EngineStructure) =
        structure |> getGears |> List.map getGearRatio |> List.sum
