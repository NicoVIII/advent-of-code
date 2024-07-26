open Expecto

open Tests
open Day02

[<EntryPoint>]
let main args =
    {
        day = 2
        part1 = part1 >> string
        part2 = part2 >> string
        readInput = Input.read
    }
    |> Tests.build<Input>
    |> runTestsWithCLIArgs [] args
 