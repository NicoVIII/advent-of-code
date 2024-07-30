namespace Day06

[<AutoOpen>]
module Expose =
    open System.IO

    let readInput = File.ReadAllText >> parseInput
    let part1 = List.map beatRecordCount >> List.map uint >> List.reduce (*)

module Program =
    [<EntryPoint>]
    let main args =
        let input = readInput args[0]

        part1 input |> printfn "Part 1: %i"
        0
