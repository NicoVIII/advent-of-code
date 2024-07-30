module Day07.Test

open Expecto

open Tests

[<Tests>]
let integrationTests =
    {
        day = 7
        part1 = part1 >> string
        part2 = fun _ -> failwith "Not implemented"
        readInput = readInput
    }
    |> Tests.build

[<EntryPoint>]
let main argv = runTestsInAssemblyWithCLIArgs [] argv
