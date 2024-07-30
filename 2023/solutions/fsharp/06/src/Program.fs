namespace Day06

open Microsoft.FSharp.Core.Operators.Checked

[<AutoOpen>]
module Expose =
    open System.IO

    let readInput = File.ReadAllText
    let part1 = parseInputV1 >> List.map beatRecordCount >> List.reduce (*)
    let part2 = parseInputV2 >> beatRecordCount

module Program =
    [<EntryPoint>]
    let main args =
        let input = readInput args[0]

        part1 input |> printfn "Part 1: %i"
        part2 input |> printfn "Part 2: %i"
        0
