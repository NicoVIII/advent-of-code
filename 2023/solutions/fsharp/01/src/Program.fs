namespace Day01

[<AutoOpen>]
module Expose =
    open System.IO

    let readInput input =
        File.ReadLines input |> Seq.cache

    let part1 = Seq.map extractFirstAndLastDigit >> Seq.sum
    let part2 = Seq.map (replaceStringByDigit >> extractFirstAndLastDigit) >> Seq.sum

module Program =
    [<EntryPoint>]
    let main args =
        if args.Length <> 0 then
            failwith "Please provide input file as only argument"

        let input = readInput args[0]

        part1 input |> printfn "Part 1: %d"
        part2 input |> printfn "Part 2: %d"
        0
