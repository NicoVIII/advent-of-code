module Day04.Program

open System.IO

let input = File.ReadLines("input.txt") |> parseInput |> Seq.cache

// Part 1
input |> Seq.map calculateWrongScore |> Seq.sum |> printfn "Part 1: %d"

// Part 2
input |> simulateWins |> printfn "Part 2: %d"
