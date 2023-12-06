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

type Maps = {
    seedToSoil: Map<Seed, Soil>
    soilToFertilizer: Map<Soil, Fertilizer>
    fertilizerToWater: Map<Fertilizer, Water>
    waterToLight: Map<Water, Light>
    lightToTemperature: Map<Light, Temperature>
    temperatureToHumidity: Map<Temperature, Humidity>
    humidityToLocation: Map<Humidity, Location>
}
