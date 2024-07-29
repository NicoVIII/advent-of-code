namespace Day05

[<AutoOpen>]
module Expose =
    open System.IO

    let readInput = File.ReadAllText

    let part1 input =
        let seeds, maps = parseInputV1 input

        seeds
        |> lookupLocationBySeed maps
        |> List.map (fun (LocationRange location) -> location.start)
        |> List.min

    let part2 input =
        let seeds, maps = parseInputV2 input

        seeds
        |> lookupLocationBySeed maps
        |> List.map (fun (LocationRange location) -> location.start)
        |> List.min

module Program =
    [<EntryPoint>]
    let main args =
        let input = readInput args[0]

        part1 input |> printfn "Part 1: %i"
        part2 input |> printfn "Part 2: %i"
        0
