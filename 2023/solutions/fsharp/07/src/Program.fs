namespace Day07

// Ensure, that nothing overflows
open Microsoft.FSharp.Core.Operators.Checked

[<AutoOpen>]
module Expose =
    open System.IO

    let readInput = File.ReadAllText >> parseInput

    let part1 =
        List.sortBy (fst >> Hand.getHandValue)
        >> List.indexed
        >> List.sumBy (fun (index, (_, bid)) -> bid * uint (index + 1))

module Program =
    [<EntryPoint>]
    let main args =
        let input = readInput args[0]

        part1 input |> printfn "Part 1: %i"
        //part2 input |> printfn "Part 2: %i"
        0
