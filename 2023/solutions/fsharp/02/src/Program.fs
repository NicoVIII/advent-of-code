namespace Day02

[<AutoOpen>]
module Expose =
    open System.IO

    let readInput input =
        File.ReadLines input |> Seq.map parseGameLine

    let part1 input =
        input
        |> Seq.filter (isGamePossible { blue = 14u; red = 12u; green = 13u })
        |> Seq.sumBy (fun game -> game.index)

    let part2 input =
        input |> Seq.sumBy (getSmallestPossibleSet >> getSetPower)

module Program =
    [<EntryPoint>]
    let main args =
        if args.Length <> 0 then
            failwith "Please provide input file as only argument"

        let input = readInput args[0]
        part1 input |> printfn "Part 1: %i"
        part2 input |> printfn "Part 2: %i"

        0
