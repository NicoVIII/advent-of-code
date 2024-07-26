namespace Day04

open System.IO

[<AutoOpen>]
module Expose =
    let readInput = File.ReadLines >> parseInput >> Seq.cache
    let part1 = Seq.map calculateWrongScore >> Seq.sum
    let part2 = simulateWins

module Program =
    [<EntryPoint>]
    let main args =
        if args.Length <> 1 then
            failwith "Please provide a single input file path"

        let input = readInput args[0]

        // Part 1
        part1 input |> printfn "Part 1: %d"

        // Part 2
        part2 input |> printfn "Part 2: %d"
        0
