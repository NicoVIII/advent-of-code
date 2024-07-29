namespace Day05

open System

[<RequireQualifiedAccess>]
module String =
    let split (separator: string) (input: string) =
        input.Split(separator, StringSplitOptions.RemoveEmptyEntries)

    let trim (input: string) = input.Trim()

[<AutoOpen>]
module Functions =
    module Parser =
        let parseSeedsV1 input =
            let parts = String.split ":" input

            parts[1]
            |> String.trim
            |> String.split " "
            |> Array.map (fun input -> BaseRange.createFromStartLength (uint input) 1u |> SeedRange)

        let parseSeedsV2 input =
            let parts = String.split ":" input

            parts[1]
            |> String.trim
            |> String.split " "
            |> Array.chunkBySize 2
            |> Array.map (function
                | [| start; length |] ->
                    BaseRange.createFromStartLength (uint start) (uint length) |> SeedRange
                | _ -> failwith "Invalid input")

        let parseRangeInfoList input =
            let parseLine input =
                input |> String.trim |> String.split " " |> Array.map uint

            input
            |> String.trim
            |> String.split "\n"
            |> List.ofArray
            |> List.tail
            |> List.map (parseLine >> fun values -> RangeMap.create values[0] values[1] values[2])

        let parseInput parseSeeds input =
            let blocks = String.split "\n\n" input

            parseSeeds blocks[0],
            {
                seedToSoil = parseRangeInfoList blocks[1]
                soilToFertilizer = parseRangeInfoList blocks[2]
                fertilizerToWater = parseRangeInfoList blocks[3]
                waterToLight = parseRangeInfoList blocks[4]
                lightToTemperature = parseRangeInfoList blocks[5]
                temperatureToHumidity = parseRangeInfoList blocks[6]
                humidityToLocation = parseRangeInfoList blocks[7]
            }

    let parseInputV1 = Parser.parseInput (Parser.parseSeedsV1 >> List.ofArray)
    let parseInputV2 = Parser.parseInput (Parser.parseSeedsV2 >> List.ofArray)

    let startToDestination map range = {
        start = (int64 range.start + map.offset) |> uint
        ``end`` = (int64 range.``end`` + map.offset) |> uint
    }

    let genericLookup rangeMapList rangeList =
        let mutable sourceRangeList = rangeList
        let mutable destinationRangeList = []

        for rangeMap in rangeMapList do
            for sourceRange in sourceRangeList do
                let destinationRange = {
                    start = max sourceRange.start rangeMap.sourceStart
                    ``end`` = min sourceRange.``end`` rangeMap.sourceEnd
                }

                if destinationRange.start <= destinationRange.``end`` then
                    destinationRangeList <-
                        startToDestination rangeMap destinationRange :: destinationRangeList

                    sourceRangeList <- List.except [ sourceRange ] sourceRangeList

                    // Do we have a source range at the start left?
                    if sourceRange.start < destinationRange.start then
                        sourceRangeList <-
                            {
                                sourceRange with
                                    ``end`` = destinationRange.start - 1u
                            }
                            :: sourceRangeList
                    // Do we have a source range at the end left?
                    if sourceRange.``end`` > destinationRange.``end`` then
                        sourceRangeList <-
                            {
                                sourceRange with
                                    start = destinationRange.``end`` + 1u
                            }
                            :: sourceRangeList

        // The remaining source Ranges are not handled by any map, therefore we simply use them as-is
        destinationRangeList @ sourceRangeList

    let lookupSoil maps =
        List.map SeedRange.getValue
        >> genericLookup maps.seedToSoil
        >> List.map SoilRange

    let lookupFertilizer maps =
        List.map SoilRange.getValue
        >> genericLookup maps.soilToFertilizer
        >> List.map FertilizerRange

    let lookupWater maps =
        List.map FertilizerRange.getValue
        >> genericLookup maps.fertilizerToWater
        >> List.map WaterRange

    let lookupLight maps =
        List.map WaterRange.getValue
        >> genericLookup maps.waterToLight
        >> List.map LightRange

    let lookupTemperature maps =
        List.map LightRange.getValue
        >> genericLookup maps.lightToTemperature
        >> List.map TemperatureRange

    let lookupHumidity maps =
        List.map TemperatureRange.getValue
        >> genericLookup maps.temperatureToHumidity
        >> List.map HumidityRange

    let lookupLocation maps =
        List.map HumidityRange.getValue
        >> genericLookup maps.humidityToLocation
        >> List.map LocationRange

    let lookupLocationBySeed maps =
        lookupSoil maps
        >> lookupFertilizer maps
        >> lookupWater maps
        >> lookupLight maps
        >> lookupTemperature maps
        >> lookupHumidity maps
        >> lookupLocation maps
