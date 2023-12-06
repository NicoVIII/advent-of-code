namespace Day05

[<AutoOpen>]
module Expose =
    open System.IO

    let readInput = File.ReadAllText >> parseInput

    let part1 (seeds, maps) =
        seeds
        |> List.map (lookupLocationBySeed maps)
        |> List.minBy (fun (Location location) -> location)

module Program =
    [<EntryPoint>]
    let main args =
        let input = readInput args[0]

        part1 input |> Location.getValue |> printfn "Part 1: %i"
        0
