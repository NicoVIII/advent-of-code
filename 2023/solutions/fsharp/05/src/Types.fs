namespace Day05

type Base = uint
type Seed = Seed of Base
type Soil = Soil of Base
type Fertilizer = Fertilizer of Base
type Water = Water of Base
type Light = Light of Base
type Temperature = Temperature of Base
type Humidity = Humidity of Base
type Location = Location of Base

type RangeInfo = {
    sourceStart: Base
    destinationStart: Base
    rangeLength: uint
}

type SortedRangeInfoList = private SortedRangeInfoList of RangeInfo list

type Maps = {
    seedToSoil: SortedRangeInfoList
    soilToFertilizer: SortedRangeInfoList
    fertilizerToWater: SortedRangeInfoList
    waterToLight: SortedRangeInfoList
    lightToTemperature: SortedRangeInfoList
    temperatureToHumidity: SortedRangeInfoList
    humidityToLocation: SortedRangeInfoList
}

[<RequireQualifiedAccess>]
module Seed =
    let getValue (Seed value) = value


[<RequireQualifiedAccess>]
module Soil =
    let getValue (Soil value) = value

[<RequireQualifiedAccess>]
module Fertilizer =
    let getValue (Fertilizer value) = value

[<RequireQualifiedAccess>]
module Water =
    let getValue (Water value) = value

[<RequireQualifiedAccess>]
module Light =
    let getValue (Light value) = value

[<RequireQualifiedAccess>]
module Temperature =
    let getValue (Temperature value) = value

[<RequireQualifiedAccess>]
module Humidity =
    let getValue (Humidity value) = value

[<RequireQualifiedAccess>]
module Location =
    let getValue (Location value) = value

[<RequireQualifiedAccess>]
module SortedRangeInfoList =
    let create = List.sortBy _.sourceStart >> SortedRangeInfoList
    let getAt index (SortedRangeInfoList list) = list[index]
    let length (SortedRangeInfoList list) = List.length list
