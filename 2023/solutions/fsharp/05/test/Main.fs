module Day05.Test

open Expecto

open Tests

[<Tests>]
let integrationTests =
    {
        day = 05
        part1 = part1 >> string
        part2 = part2 >> string
        readInput = readInput
    }
    |> Tests.build

[<EntryPoint>]
let main argv = runTestsInAssemblyWithCLIArgs [] argv
