module Day03.Test

open Expecto
open Tests

[<Tests>]
let unitTests =
    testList "Functions" [
        test "getNumbersAdjacentToSymbol" {
            let input = [|
                "467..114.."
                "...*......"
                "..35..633."
                "......#..."
                "617*......"
                ".....+.58."
                "..592....."
                "......755."
                "...$.*...."
                ".664.598.."
            |]

            let expected = [ 467u; 35u; 633u; 617u; 592u; 755u; 664u; 598u ]

            let actual = input |> parseSchematic |> getNumbersAdjacentToSymbol

            Expect.equal actual expected "Expected a different result"
        }
        test "getGearRatioSum" {
            let input = [|
                "467..114.."
                "...*......"
                "..35..633."
                "......#..."
                "617*......"
                ".....+.58."
                "..592....."
                "......755."
                "...$.*...."
                ".664.598.."
            |]

            let expected = 467835u

            let actual = input |> parseSchematic |> getGearRatioSum

            Expect.equal actual expected "Expected a different result"
        }
    ]

[<Tests>]
let integrationTests =
    {
        day = 3
        part1 = part1 >> string
        part2 = part2 >> string
        readInput = readInput
    }
    |> Tests.build

[<EntryPoint>]
let main argv = runTestsInAssemblyWithCLIArgs [] argv
