namespace Day05

open System

[<RequireQualifiedAccess>]
module String =
    let split (separator: string) (input: string) =
        input.Split(separator, StringSplitOptions.RemoveEmptyEntries)

    let trim (input: string) = input.Trim()

[<AutoOpen>]
module Functions =
    let parseInput (input: string) =
        let parseSeeds input =
            let parts = String.split ":" input

            parts[1]
            |> String.trim
            |> String.split " "
            |> Array.map (uint >> Seed)
            |> List.ofArray

        let parseMap keyConstr valueConstr input =
            let parseLine input =
                input |> String.trim |> String.split " " |> Array.map uint

            input
            |> String.trim
            |> String.split "\n"
            |> List.ofArray
            |> List.tail
            |> List.map parseLine
            |> List.collect (fun values ->
                let valueStart = values[0]
                let keyStart = values[1]
                let range = values[2]

                [
                    for i in 0u .. range - 1u do
                        keyConstr (keyStart + i), valueConstr (valueStart + i)
                ])
            |> Map.ofList

        let blocks = String.split "\n\n" input

        parseSeeds blocks[0],
        {
            seedToSoil = parseMap Seed Soil blocks[1]
            soilToFertilizer = parseMap Soil Fertilizer blocks[2]
            fertilizerToWater = parseMap Fertilizer Water blocks[3]
            waterToLight = parseMap Water Light blocks[4]
            lightToTemperature = parseMap Light Temperature blocks[5]
            temperatureToHumidity = parseMap Temperature Humidity blocks[6]
            humidityToLocation = parseMap Humidity Location blocks[7]
        }

    let lookupSoil maps seed =
        match Map.tryFind seed maps.seedToSoil with
        | Some soil -> soil
        | None ->
            let (Seed value) = seed
            Soil value

    let lookupFertilizer maps soil =
        match Map.tryFind soil maps.soilToFertilizer with
        | Some fertilizer -> fertilizer
        | None ->
            let (Soil value) = soil
            Fertilizer value

    let lookupWater maps fertilizer =
        match Map.tryFind fertilizer maps.fertilizerToWater with
        | Some water -> water
        | None ->
            let (Fertilizer value) = fertilizer
            Water value

    let lookupLight maps water =
        match Map.tryFind water maps.waterToLight with
        | Some light -> light
        | None ->
            let (Water value) = water
            Light value

    let lookupTemperature maps light =
        match Map.tryFind light maps.lightToTemperature with
        | Some temperature -> temperature
        | None ->
            let (Light value) = light
            Temperature value

    let lookupHumidity maps temperature =
        match Map.tryFind temperature maps.temperatureToHumidity with
        | Some humidity -> humidity
        | None ->
            let (Temperature value) = temperature
            Humidity value

    let lookupLocation maps humidity =
        match Map.tryFind humidity maps.humidityToLocation with
        | Some location -> location
        | None ->
            let (Humidity value) = humidity
            Location value

    let lookupLocationBySeed maps seed =
        seed
        |> lookupSoil maps
        |> lookupFertilizer maps
        |> lookupWater maps
        |> lookupLight maps
        |> lookupTemperature maps
        |> lookupHumidity maps
        |> lookupLocation maps
