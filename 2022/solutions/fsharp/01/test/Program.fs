open Expecto

open Tests
open Day01

[<EntryPoint>]
let main args =
    {
        day = 1
        part1 = part1 >> string
        part2 = part2 >> string
        readInput = Input.prepare
    }
    |> Tests.build<Input>
    |> runTestsWithCLIArgs [] args
