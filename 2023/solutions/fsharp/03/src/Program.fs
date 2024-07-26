namespace Day03

[<AutoOpen>]
module Expose =
    open System.IO

    let readInput = File.ReadAllLines >> parseSchematic
    let part1 = getNumbersAdjacentToSymbol >> List.sum
    let part2 = getGearRatioSum

module Program =
    [<EntryPoint>]
    let main args =
        let input = readInput args[0]

        part1 input |> printfn "Part 1: %d"
        part2 input |> printfn "Part 2: %d"
        0
