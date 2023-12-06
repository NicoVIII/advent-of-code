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
        let parseSeeds input =
            let parts = String.split ":" input

            parts[1]
            |> String.trim
            |> String.split " "
            |> Array.map (uint >> Seed)
            |> List.ofArray

        let parseRangeInfoList input =
            let parseLine input =
                input |> String.trim |> String.split " " |> Array.map uint

            input
            |> String.trim
            |> String.split "\n"
            |> List.ofArray
            |> List.tail
            |> List.map (
                parseLine
                >> fun values -> {
                    destinationStart = values[0]
                    sourceStart = values[1]
                    rangeLength = values[2]
                }
            )
            |> SortedRangeInfoList.create

    let parseInput (input: string) =
        let blocks = String.split "\n\n" input

        Parser.parseSeeds blocks[0],
        {
            seedToSoil = Parser.parseRangeInfoList blocks[1]
            soilToFertilizer = Parser.parseRangeInfoList blocks[2]
            fertilizerToWater = Parser.parseRangeInfoList blocks[3]
            waterToLight = Parser.parseRangeInfoList blocks[4]
            lightToTemperature = Parser.parseRangeInfoList blocks[5]
            temperatureToHumidity = Parser.parseRangeInfoList blocks[6]
            humidityToLocation = Parser.parseRangeInfoList blocks[7]
        }

    let genericLookup sortedRangeInfoList value =
        // Search binarily for a potentially fitting range info
        let rec search start ``end`` =
            if start > ``end`` then
                // There is no fitting info => value stays the same
                value
            else
                let middle = (start + ``end``) / 2
                let rangeInfo = SortedRangeInfoList.getAt middle sortedRangeInfoList

                if
                    value >= rangeInfo.sourceStart
                    && value < rangeInfo.sourceStart + rangeInfo.rangeLength
                then
                    // We found a fitting info
                    rangeInfo.destinationStart + value - rangeInfo.sourceStart
                elif rangeInfo.sourceStart > value then
                    search start (middle - 1)
                else
                    search (middle + 1) ``end``

        let length = SortedRangeInfoList.length sortedRangeInfoList
        search 0 (length - 1)

    let lookupSoil maps =
        Seed.getValue >> genericLookup maps.seedToSoil >> Soil

    let lookupFertilizer maps =
        Soil.getValue >> genericLookup maps.soilToFertilizer >> Fertilizer

    let lookupWater maps =
        Fertilizer.getValue >> genericLookup maps.fertilizerToWater >> Water

    let lookupLight maps =
        Water.getValue >> genericLookup maps.waterToLight >> Light

    let lookupTemperature maps =
        Light.getValue >> genericLookup maps.lightToTemperature >> Temperature

    let lookupHumidity maps =
        Temperature.getValue >> genericLookup maps.temperatureToHumidity >> Humidity

    let lookupLocation maps =
        Humidity.getValue >> genericLookup maps.humidityToLocation >> Location

    let lookupLocationBySeed maps =
        lookupSoil maps
        >> lookupFertilizer maps
        >> lookupWater maps
        >> lookupLight maps
        >> lookupTemperature maps
        >> lookupHumidity maps
        >> lookupLocation maps
