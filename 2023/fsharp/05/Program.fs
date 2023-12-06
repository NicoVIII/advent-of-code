module Day05.Program

open System.IO

// Part 1
let seeds, maps = File.ReadAllText("input.txt") |> parseInput

seeds
|> List.map (lookupLocationBySeed maps)
|> List.minBy (fun (Location location) -> location)
|> printfn "Part 1: %A"
