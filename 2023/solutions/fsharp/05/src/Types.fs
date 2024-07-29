namespace Day05

type Base = uint
type BaseRange = { start: Base; ``end``: Base }
type SeedRange = SeedRange of BaseRange
type SoilRange = SoilRange of BaseRange
type FertilizerRange = FertilizerRange of BaseRange
type WaterRange = WaterRange of BaseRange
type LightRange = LightRange of BaseRange
type TemperatureRange = TemperatureRange of BaseRange
type HumidityRange = HumidityRange of BaseRange
type LocationRange = LocationRange of BaseRange

type RangeMap = private {
    sourceStart: Base
    sourceEnd: Base
    offset: int64
}

type InputMaps = {
    seedToSoil: RangeMap list
    soilToFertilizer: RangeMap list
    fertilizerToWater: RangeMap list
    waterToLight: RangeMap list
    lightToTemperature: RangeMap list
    temperatureToHumidity: RangeMap list
    humidityToLocation: RangeMap list
}

[<RequireQualifiedAccess>]
module BaseRange =
    let createFromStartLength start length = {
        start = start
        ``end`` = start + length - 1u
    }

[<RequireQualifiedAccess>]
module SeedRange =
    let getValue (SeedRange value) = value

[<RequireQualifiedAccess>]
module SoilRange =
    let getValue (SoilRange value) = value

[<RequireQualifiedAccess>]
module FertilizerRange =
    let getValue (FertilizerRange value) = value

[<RequireQualifiedAccess>]
module WaterRange =
    let getValue (WaterRange value) = value

[<RequireQualifiedAccess>]
module LightRange =
    let getValue (LightRange value) = value

[<RequireQualifiedAccess>]
module TemperatureRange =
    let getValue (TemperatureRange value) = value

[<RequireQualifiedAccess>]
module HumidityRange =
    let getValue (HumidityRange value) = value

[<RequireQualifiedAccess>]
module LocationRange =
    let getValue (LocationRange value) = value

[<RequireQualifiedAccess>]
module RangeMap =
    let create (destinationStart: Base) sourceStart length = {
        sourceStart = sourceStart
        sourceEnd = sourceStart + length - 1u
        offset = int64 destinationStart - int64 sourceStart
    }
