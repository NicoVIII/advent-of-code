module Day01.Program

open System.IO

let input = File.ReadLines("input.txt") |> Seq.cache

// Part 1
input |> Seq.map extractFirstAndLastDigit |> Seq.sum |> printfn "Part 1: %d"

// Part 2
input
|> Seq.map (replaceStringByDigit >> extractFirstAndLastDigit)
|> Seq.sum
|> printfn "Part 2: %d"
