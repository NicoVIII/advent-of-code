module Day03.Program

open System.IO

let input = File.ReadAllLines("input.txt") |> parseSchematic

// Part 1
input |> getNumbersAdjacentToSymbol |> List.sum |> printfn "Part 1: %d"

// Part 2
input |> getGearRatioSum |> printfn "Part 2: %d"
