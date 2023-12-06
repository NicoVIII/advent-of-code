module Day05.Test

open Expecto

open Tests

[<Tests>]
let unitTests =
    testList "Functions" [
        testList "Parser" [
            testList (nameof Parser.parseSeeds) [
                test "simple example" {
                    let input = "seeds: 79 14 55 13"
                    let expected = [ 79u; 14u; 55u; 13u ] |> List.map Seed
                    let actual = input |> Parser.parseSeeds

                    Expect.equal actual expected "Expected a different result"
                }
            ]
        ]
    ]

[<Tests>]
let integrationTests =
    {
        day = 05
        part1 = part1 >> Location.getValue >> string
        part2 = fun _ -> failwith "Not implemented"
        readInput = readInput
    }
    |> Tests.build

[<EntryPoint>]
let main argv = runTestsInAssemblyWithCLIArgs [] argv
